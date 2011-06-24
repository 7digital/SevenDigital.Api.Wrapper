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

		public Uri SignUrl(string urlWithParameters, string userToken, string userSecret, IOAuthCredentials consumerCredentials)
		{
			var timestamp = _oAuthBase.GenerateTimeStamp();
			var nonce = _oAuthBase.GenerateNonce();

			string normalizedRequestParameters;
			string normalizedUrl;
			var signature = _oAuthBase.GenerateSignature(new Uri(urlWithParameters),
																consumerCredentials.ConsumerKey,
																consumerCredentials.ConsumerSecret, 
																userToken, 
																userSecret, 
																"GET", 
																timestamp, 
																nonce, 
																out normalizedUrl, 
																out normalizedRequestParameters, 
																new Dictionary<string, string>());

			string escapeDataString = Uri.EscapeDataString(signature);
			return new Uri(string.Format("{0}?{1}&oauth_signature={2}", normalizedUrl, normalizedRequestParameters, escapeDataString));
		}

        public IDictionary<string, string> SignPostRequest(string url, string userToken, string userSecret, 
            IOAuthCredentials consumerCredentials, Dictionary<string, string> postParameters)
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
                consumerCredentials.ConsumerSecret, userToken, userSecret, "POST", timestamp, nonce, 
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
