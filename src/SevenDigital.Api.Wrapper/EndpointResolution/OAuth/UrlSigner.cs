using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
	}

	
}