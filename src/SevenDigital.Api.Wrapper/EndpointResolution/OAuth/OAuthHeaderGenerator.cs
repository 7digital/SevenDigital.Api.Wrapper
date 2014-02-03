using OAuth;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public class OAuthHeaderGenerator
	{
		private readonly IOAuthCredentials _oAuthCredentials;

		public OAuthHeaderGenerator(IOAuthCredentials oAuthCredentials)
		{
			_oAuthCredentials = oAuthCredentials;
		}

		public string GenerateOAuthSignatureHeader(OAuthHeaderData data)
		{
			// request body
			// content-type

			var oauthRequest = new OAuthRequest
			{
				Type = OAuthRequestType.ProtectedResource,
				RequestUrl = data.Url,
				Method = data.HttpMethod.ToString().ToUpperInvariant(),
				ConsumerKey = _oAuthCredentials.ConsumerKey,
				ConsumerSecret = _oAuthCredentials.ConsumerSecret,
			};

			if (!string.IsNullOrEmpty(data.UserToken))
			{
				oauthRequest.Token = data.UserToken;
				oauthRequest.TokenSecret = data.TokenSecret;
			}

			return oauthRequest.GetAuthorizationHeader(data.RequestParameters);
		}
	}
}