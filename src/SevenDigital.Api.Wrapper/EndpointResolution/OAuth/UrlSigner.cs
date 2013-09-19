using System;
using System.Collections.Generic;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public class UrlSigner : IUrlSigner
	{
		private readonly ISignatureGenerator _signatureGenerator;

		public UrlSigner(ISignatureGenerator signatureGenerator)
		{
			_signatureGenerator = signatureGenerator;
		}

		/// <summary>
		/// If you need to sign a url and get it as a string, use this method instead of 
		/// using SignUrl() returning a Uri.  If you use SignUrl(), as soon as you turn 
		/// the Uri into a string, it unescapes the oauth signature and if it contains '+' characters
		/// it will fail.
		/// </summary>
		public string SignGetUrl(string urlWithParameters, string userToken, string tokenSecret, IOAuthCredentials consumerCredentials)
		{
			var oAuthSignatureInfo = new OAuthSignatureInfo
			{
				FullUrlToSign = urlWithParameters,
				ConsumerCredentials = consumerCredentials,
				HttpMethod = "GET",
				UserAccessToken = new OAuthAccessToken { Token = userToken, Secret = tokenSecret}
			};
			return _signatureGenerator.Sign(oAuthSignatureInfo);
		}

		public Uri SignUrl(string urlWithParameters, string userToken, string tokenSecret, IOAuthCredentials consumerCredentials)
		{
			return new Uri(SignGetUrl(urlWithParameters, userToken, tokenSecret, consumerCredentials));
		}

		public IDictionary<string, string> SignPostRequest(string url, string userToken, string tokenSecret, IOAuthCredentials consumerCredentials, IDictionary<string, string> postParameters)
		{
			if (string.IsNullOrEmpty(consumerCredentials.ConsumerKey))
				throw new ArgumentException("ConsumerKey can not be null or empty");

			if (string.IsNullOrEmpty(consumerCredentials.ConsumerSecret))
				throw new ArgumentException("ConsumerSecret can not be null or empty");

			var oAuthSignatureInfo = new OAuthSignatureInfo
			{
				FullUrlToSign = url,
				ConsumerCredentials = consumerCredentials,
				HttpMethod = "POST",
				UserAccessToken = new OAuthAccessToken { Token = userToken, Secret = tokenSecret },
				PostData = postParameters
			};
			return _signatureGenerator.SignWithPostData(oAuthSignatureInfo);
		}
	}

}
