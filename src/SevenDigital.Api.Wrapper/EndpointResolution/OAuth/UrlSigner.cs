using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public class UrlSigner : IUrlSigner
	{
		private readonly OAuthBase _oAuthBase;
		
		public UrlSigner()
		{
			_oAuthBase = new OAuthBase();
		}

		/// <summary>
		/// If you need to sign a url and get it as a string, use this method instead of 
		/// using SignUrl() returning a Uri.  If you use SignUrl(), as soon as you turn 
		/// the Uri into a string, it unescapes the oauth signature and if it contains '+' characters
		/// it will fail.
		/// </summary>
		public string SignGetUrl(string urlWithParameters, string userToken, string tokenSecret, IOAuthCredentials consumerCredentials)
		{
			var timeStamp = _oAuthBase.GenerateTimeStamp();
			var nonce = _oAuthBase.GenerateNonce();

			string normalizedRequestParameters;
			string normalizedUrl;
			var signature = _oAuthBase.GenerateSignature(new Uri(urlWithParameters),
				consumerCredentials.ConsumerKey,
				consumerCredentials.ConsumerSecret,
				userToken,
				tokenSecret,
				"GET",
				timeStamp,
				nonce,
				out normalizedUrl,
				out normalizedRequestParameters,
				new Dictionary<string, string>());

			var encodedSignature = OAuthBase.UrlEncode(signature);
			return string.Format("{0}?{1}&oauth_signature={2}", normalizedUrl, normalizedRequestParameters, encodedSignature);
		}

		public Uri SignUrl(string urlWithParameters, string userToken, string tokenSecret, IOAuthCredentials consumerCredentials)
		{
			return new Uri(SignGetUrl(urlWithParameters, userToken, tokenSecret, consumerCredentials));
		}

		public IDictionary<string, string> SignPostRequest(string url, string userToken, string tokenSecret,
			IOAuthCredentials consumerCredentials, IDictionary<string, string> postParameters)
		{
			if (string.IsNullOrEmpty(consumerCredentials.ConsumerKey))
				throw new ArgumentException("ConsumerKey can not be null or empty");

			if (string.IsNullOrEmpty(consumerCredentials.ConsumerSecret))
				throw new ArgumentException("ConsumerSecret can not be null or empty");

			var timestamp = _oAuthBase.GenerateTimeStamp();
			var nonce = _oAuthBase.GenerateNonce();

			string normalizedRequestParameters;
			string normalizedUrl;
			
			var signature = _oAuthBase.GenerateSignature(new Uri(url), consumerCredentials.ConsumerKey, 
				consumerCredentials.ConsumerSecret, userToken, tokenSecret, "POST", timestamp, nonce,
				out normalizedUrl, out normalizedRequestParameters, postParameters);

			var parameters = new Dictionary<string, string>(postParameters)
			{
				{ OAuthBase.OAuthVersionKey, OAuthBase.OAuthVersion },
				{ OAuthBase.OAuthNonceKey, nonce },
				{ OAuthBase.OAuthTimestampKey, timestamp },
				{ OAuthBase.OAuthSignatureMethodKey, OAuthBase.HMACSHA1SignatureType },
				{ OAuthBase.OAuthConsumerKeyKey, consumerCredentials.ConsumerKey },
				{ OAuthBase.OAuthSignatureKey, OAuthBase.UrlEncode(signature) }
			};

			if (!string.IsNullOrEmpty(userToken))
			{
				parameters.Add(OAuthBase.OAuthTokenKey, userToken);
			}

			return parameters;
		}
	}

}
