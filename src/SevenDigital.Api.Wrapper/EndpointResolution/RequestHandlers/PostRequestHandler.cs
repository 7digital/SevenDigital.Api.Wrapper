using System.Collections.Generic;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class PostRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;

		public PostRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials)
			: base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
		}

		public override bool HandlesMethod(HttpMethod method)
		{
			return method == HttpMethod.Post;
		}

		public override Response HitEndpoint(RequestData requestData)
		{
			var postRequest = BuildPostRequest(requestData);
			return HttpClient.Send(postRequest);
		}

		private Request BuildPostRequest(RequestData requestData)
		{
			var apiRequest = MakeApiRequest(requestData);

			if (requestData.RequiresSignature)
			{
				var oauthHeader = BuildOAuthHeader(requestData, apiRequest.AbsoluteUrl, apiRequest.Parameters);
				requestData.Headers.Add("Authorization", oauthHeader);
			}
			else
			{
				requestData.Headers.Add("Authorization", "oauth_consumer_key=" + _oAuthCredentials.ConsumerKey);
			}

			string requestBody = apiRequest.Parameters.ToQueryString();
			return new Request(HttpMethod.Post, apiRequest.AbsoluteUrl, requestData.Headers, requestBody);
		}

		private string BuildOAuthHeader(RequestData requestData, string fullUrl, IDictionary<string, string> parameters)
		{
			var authHeaderGenerator = new OAuthHeaderGenerator(_oAuthCredentials);
			var oAuthHeaderData = new OAuthHeaderData
				{
					Url = fullUrl,
					HttpMethod = HttpMethod.Post,
					UserToken = requestData.UserToken,
					TokenSecret = requestData.TokenSecret,
					RequestParameters = parameters
				};
			return authHeaderGenerator.GenerateOAuthSignatureHeader(oAuthHeaderData);
		}

		public override string GetDebugUri(RequestData requestData)
		{
			return MakeApiRequest(requestData).FullUri;
		}
	}
}