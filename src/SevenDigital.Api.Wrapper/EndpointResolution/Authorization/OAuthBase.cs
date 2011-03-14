using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	public class OAuthBase
	{
		private readonly IHashComputer _hashComputer;

		public OAuthBase() : this(new HashComputer()) {} // TODO : IOC Container

		public OAuthBase(IHashComputer hashComputer)
		{
			_hashComputer = hashComputer;
		}

		protected string NormalizeRequestParameters(IList<QueryParameter> parameters)
		{
			var sb = new StringBuilder();

			for (int i = 0; i < parameters.Count; i++)
			{
				QueryParameter p = parameters[i];
				sb.AppendFormat("{0}={1}", p.Name, p.Name.StartsWith("oauth") 
				                                   	? HttpUtility.UrlEncode(p.Value) 
				                                   	: p.Value);

				if (i < parameters.Count - 1)
					sb.Append("&");
			}
			return sb.ToString();
		}

		public string GenerateSignatureBase(OAuthSignatureParameters oAuthSignatureParameters, out string normalizedUrl, out string normalizedRequestParameters)
		{
			if (oAuthSignatureParameters.Token == null)
				oAuthSignatureParameters.Token = string.Empty;

			Validate(oAuthSignatureParameters);

			normalizedUrl = null;
			normalizedRequestParameters = null;

			var parameters = GetQueryParameters(oAuthSignatureParameters.Url.Query).ToList();

			if (!String.IsNullOrEmpty(oAuthSignatureParameters.OAuthVersion))
				parameters.Add(new QueryParameter(OAuthConstants.O_AUTH_VERSION_KEY, oAuthSignatureParameters.OAuthVersion));

			if (oAuthSignatureParameters.PostParameters != null & oAuthSignatureParameters.HttpMethod.ToUpper() == "POST")
				parameters.AddRange(oAuthSignatureParameters.PostParameters.Keys.Select(key => new QueryParameter(key, oAuthSignatureParameters.PostParameters[key])));

			parameters.Add(new QueryParameter(OAuthConstants.O_AUTH_NONCE_KEY, oAuthSignatureParameters.Nonce));
			parameters.Add(new QueryParameter(OAuthConstants.O_AUTH_TIMESTAMP_KEY, oAuthSignatureParameters.TimeStamp));
			parameters.Add(new QueryParameter(OAuthConstants.O_AUTH_SIGNATURE_METHOD_KEY, oAuthSignatureParameters.SignatureType.ToString()));
			parameters.Add(new QueryParameter(OAuthConstants.OAUTH_CONSUMER_KEY_KEY, oAuthSignatureParameters.ConsumerKey));

			if (!string.IsNullOrEmpty(oAuthSignatureParameters.Token))
				parameters.Add(new QueryParameter(OAuthConstants.O_AUTH_TOKEN_KEY, oAuthSignatureParameters.Token));

			parameters.Sort(new QueryParameterComparer());

			normalizedUrl = string.Format("{0}://{1}", oAuthSignatureParameters.Url.Scheme, oAuthSignatureParameters.Url.Host);
			if (!((oAuthSignatureParameters.Url.Scheme == "http" && oAuthSignatureParameters.Url.Port == 80) 
				|| (oAuthSignatureParameters.Url.Scheme == "https" && oAuthSignatureParameters.Url.Port == 443)))
				normalizedUrl += string.Format(":{0}", oAuthSignatureParameters.Url.Port);

			normalizedUrl += oAuthSignatureParameters.Url.AbsolutePath;
			normalizedRequestParameters = NormalizeRequestParameters(parameters);

			var signatureBase = new StringBuilder();
			signatureBase.AppendFormat("{0}&", oAuthSignatureParameters.HttpMethod.ToUpper());
			signatureBase.AppendFormat("{0}&", HttpUtility.UrlEncode(normalizedUrl));
			signatureBase.AppendFormat("{0}", HttpUtility.UrlEncode(normalizedRequestParameters));
			return signatureBase.ToString();
		}

		public string GenerateSignature(OAuthSignatureParameters oAuthSignatureParameters, out string normalizedUrl, out string normalizedRequestParameters)
		{
			normalizedUrl = null;
			normalizedRequestParameters = null;

			switch (oAuthSignatureParameters.SignatureType)
			{
				case SignatureTypes.PLAINTEXT:
					return HttpUtility.UrlEncode(string.Format("{0}&{1}", oAuthSignatureParameters.ConsumerSecret, oAuthSignatureParameters.TokenSecret));
				case SignatureTypes.HMACSHA1:
					string signatureBase = GenerateSignatureBase(oAuthSignatureParameters, out normalizedUrl, out normalizedRequestParameters);

					var hmacsha1 = new HMACSHA1
					{
						Key = GetKey(oAuthSignatureParameters.ConsumerSecret, oAuthSignatureParameters.TokenSecret)
					};
					return _hashComputer.Compute(hmacsha1, signatureBase);

				default:
					throw new NotImplementedException();
			}
		}

		private static void Validate(OAuthSignatureParameters oAuthSignatureParameters)
		{
			if (string.IsNullOrEmpty(oAuthSignatureParameters.ConsumerKey))
				throw new ArgumentNullException("consumerKey");

			if (string.IsNullOrEmpty(oAuthSignatureParameters.HttpMethod))
				throw new ArgumentNullException("httpMethod");
		}

		private static IEnumerable<QueryParameter> GetQueryParameters(string parameters)
		{
			if (parameters.StartsWith("?"))
				parameters = parameters.Remove(0, 1);

			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(parameters);
			
			return from string key in nameValueCollection 
				   where (!string.IsNullOrEmpty(key) && !key.StartsWith(OAuthConstants.O_AUTH_PARAMETER_PREFIX)) 
				   select new QueryParameter(key, nameValueCollection[key]);
		}

		private static byte[] GetKey(string consumerSecret, string tokenSecret)
		{
			return Encoding.ASCII.GetBytes(string.Format("{0}&{1}", HttpUtility.UrlEncode(consumerSecret),
			                                             string.IsNullOrEmpty(tokenSecret)
			                                             	? ""
															: HttpUtility.UrlEncode(tokenSecret)));
		}
	}
}
