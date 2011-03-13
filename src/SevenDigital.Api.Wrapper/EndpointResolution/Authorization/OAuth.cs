using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	public class OAuthSignatureParameters
	{
		public Uri Url { get;  set; }
		public string ConsumerKey { get;  set; }
		public string ConsumerSecret { get;  set; }
		public string Token { get; set; }
		public string TokenSecret { get;  set; }
		public string HttpMethod { get;  set; }
		public string TimeStamp { get;  set; }
		public string Nonce { get;  set; }
		public SignatureTypes SignatureType { get;  set; }
		public IDictionary<string, string> PostParameters { get;  set; }
		public string OAuthVersion { get;  set; }
	}

	public class OAuthBase
	{
		public const string O_AUTH_VERSION = "1.0";
		protected const string O_AUTH_PARAMETER_PREFIX = "oauth_";

		public bool IncludeVersion = true;

		public const string OAUTH_CONSUMER_KEY_KEY = "oauth_consumer_key";
		public const string O_AUTH_CALLBACK_KEY = "oauth_callback";
		public const string O_AUTH_VERSION_KEY = "oauth_version";
		public const string O_AUTH_SIGNATURE_METHOD_KEY = "oauth_signature_method";
		public const string O_AUTH_SIGNATURE_KEY = "oauth_signature";
		public const string O_AUTH_TIMESTAMP_KEY = "oauth_timestamp";
		public const string O_AUTH_NONCE_KEY = "oauth_nonce";
		public const string O_AUTH_TOKEN_KEY = "oauth_token";
		public const string O_AUTH_TOKEN_SECRET_KEY = "oauth_token_secret";
		public const string HMACSHA1_SIGNATURE_TYPE = "HMAC-SHA1";
		public const string PLAIN_TEXT_SIGNATURE_TYPE = "PLAINTEXT";
		public const string RSASHA1_SIGNATURE_TYPE = "RSA-SHA1";
		
		protected static string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

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
				parameters.Add(new QueryParameter(O_AUTH_VERSION_KEY, oAuthSignatureParameters.OAuthVersion));

			if (oAuthSignatureParameters.PostParameters != null & oAuthSignatureParameters.HttpMethod.ToUpper() == "POST")
				parameters.AddRange(oAuthSignatureParameters.PostParameters.Keys.Select(key => new QueryParameter(key, oAuthSignatureParameters.PostParameters[key])));

			parameters.Add(new QueryParameter(O_AUTH_NONCE_KEY, oAuthSignatureParameters.Nonce));
			parameters.Add(new QueryParameter(O_AUTH_TIMESTAMP_KEY, oAuthSignatureParameters.TimeStamp));
			parameters.Add(new QueryParameter(O_AUTH_SIGNATURE_METHOD_KEY, oAuthSignatureParameters.SignatureType.ToString()));
			parameters.Add(new QueryParameter(OAUTH_CONSUMER_KEY_KEY, oAuthSignatureParameters.ConsumerKey));

			if (!string.IsNullOrEmpty(oAuthSignatureParameters.Token))
				parameters.Add(new QueryParameter(O_AUTH_TOKEN_KEY, oAuthSignatureParameters.Token));

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
					return ComputeHash(hmacsha1, signatureBase);

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

		private static string ComputeHash(HashAlgorithm hashAlgorithm, string data)
		{
			if (hashAlgorithm == null)
				throw new ArgumentNullException("hashAlgorithm");

			if (string.IsNullOrEmpty(data))
				throw new ArgumentNullException("data");

			byte[] dataBuffer = Encoding.ASCII.GetBytes(data);
			byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

			return Convert.ToBase64String(hashBytes);
		}

		private static IEnumerable<QueryParameter> GetQueryParameters(string parameters)
		{
			if (parameters.StartsWith("?"))
				parameters = parameters.Remove(0, 1);

			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(parameters);
			
			return from string key in nameValueCollection 
				   where (!string.IsNullOrEmpty(key) && !key.StartsWith(O_AUTH_PARAMETER_PREFIX)) 
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
