using OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public class OAuthHeaderGenerator
	{
		private readonly IOAuthCredentials _oAuthCredentials;

		public OAuthHeaderGenerator(IOAuthCredentials oAuthCredentials)
		{
			_oAuthCredentials = oAuthCredentials;
		}

		public string GenerateOAuthSignatureHeader(ApiRequest apiRequest, RequestData requestData)
		{
			// request body
			// content-type

			if (!requestData.RequiresSignature)
			{
				return string.Empty;
			}

			var oauthRequest = new OAuthRequest
			{
				Type = OAuthRequestType.ProtectedResource,
				RequestUrl = apiRequest.AbsoluteUrl,
				Method = requestData.HttpMethod.ToString().ToUpperInvariant(),
				ConsumerKey = _oAuthCredentials.ConsumerKey,
				ConsumerSecret = _oAuthCredentials.ConsumerSecret,
			//	Token = requestData.UserToken,
			//	TokenSecret = requestData.TokenSecret,
			};

			return oauthRequest.GetAuthorizationHeader();
		}
	}
}