using System.Collections.Generic;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class GetRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;

		public GetRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials) : base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
		}

		public override bool HandlesMethod(HttpMethod method)
		{
			return method == HttpMethod.Get;
		}

		public override Response HitEndpoint(RequestData requestData)
		{
			var getRequest = BuildGetRequest(requestData);
			return HttpClient.Get(getRequest);
		}

		private GetRequest BuildGetRequest(RequestData requestData)
		{
			var apiRequest = MakeApiRequest(requestData);
			var fullUrl = apiRequest.AbsoluteUrl;

			if (!requestData.RequiresSignature)
			{
				apiRequest.Parameters.Add("oauth_consumer_key", _oAuthCredentials.ConsumerKey);
			}

			if (apiRequest.Parameters.Count > 0)
			{
				fullUrl += "?" + apiRequest.Parameters.ToQueryString();
			}

			if (requestData.RequiresSignature)
			{
				var oauthHeader = BuildOAuthHeader(requestData, fullUrl, apiRequest.Parameters);
				requestData.Headers.Add("Authorization", oauthHeader);
			}

			return new GetRequest(fullUrl, requestData.Headers);
		}

		private string BuildOAuthHeader(RequestData requestData, string fullUrl, IDictionary<string, string> parameters)
		{
			var authHeaderGenerator = new OAuthHeaderGenerator(_oAuthCredentials);
			var oAuthHeaderData = new OAuthHeaderData
			{
				Url = fullUrl,
				HttpMethod = HttpMethod.Get,
				UserToken = requestData.UserToken,
				TokenSecret = requestData.TokenSecret,
				RequestParameters = parameters
			};
			return authHeaderGenerator.GenerateOAuthSignatureHeader(oAuthHeaderData);
		}

		public override string GetDebugUri(RequestData requestData)
		{
			var apiRequest = MakeApiRequest(requestData);
			apiRequest.Parameters.Add("oauth_consumer_key", _oAuthCredentials.ConsumerKey);
			return apiRequest.FullUri;
		}
	}
}