using System.Collections.Generic;

using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class AllRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;

		public AllRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials)
			: base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
		}

		public override bool HandlesMethod(HttpMethod method)
		{
			return true;
		}

		public override Response HitEndpoint(RequestData requestData)
		{
			var request = BuildRequest(requestData);
			return HttpClient.Send(request);
		}

		private Request BuildRequest(RequestData requestData)
		{
			var apiRequest = MakeApiRequest(requestData);
			var fullUrl = apiRequest.AbsoluteUrl;

			if (!requestData.RequiresSignature)
			{
				if (HttpMethodHelpers.HasParams(requestData.HttpMethod))
				{
					apiRequest.Parameters.Add("oauth_consumer_key", _oAuthCredentials.ConsumerKey);
				}
				else
				{
					requestData.Headers.Add("Authorization", "oauth_consumer_key=" + _oAuthCredentials.ConsumerKey);
				}
			}

			if (HttpMethodHelpers.HasParams(requestData.HttpMethod) && (apiRequest.Parameters.Count > 0))
			{
				fullUrl += "?" + apiRequest.Parameters.ToQueryString();
			}

			if (requestData.RequiresSignature)
			{
				var oauthHeader = BuildOAuthHeader(requestData, fullUrl, apiRequest.Parameters);
				requestData.Headers.Add("Authorization", oauthHeader);
			}

			string requestBody;
			if (HttpMethodHelpers.HasBody(requestData.HttpMethod))
			{
				requestBody = apiRequest.Parameters.ToQueryString();
			}
			else
			{
				requestBody = string.Empty;
			}

			return new Request(requestData.HttpMethod, fullUrl, requestData.Headers, requestBody);
		}

		private string BuildOAuthHeader(RequestData requestData, string fullUrl, IDictionary<string, string> parameters)
		{
			var authHeaderGenerator = new OAuthHeaderGenerator(_oAuthCredentials);
			var oAuthHeaderData = new OAuthHeaderData
				{
					Url = fullUrl,
					HttpMethod = requestData.HttpMethod,
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