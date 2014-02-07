using System.Collections.Generic;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class RequestHandler : IRequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly RouteParamsSubstitutor _routeParamsSubstitutor;

		public RequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials)
		{
			_oAuthCredentials = oAuthCredentials;
			_routeParamsSubstitutor = new RouteParamsSubstitutor(apiUri);
		}

		public Request BuildRequest(RequestData requestData)
		{
			var apiRequest = _routeParamsSubstitutor.SubstituteParamsInRequest(requestData);
			var fullUrl = apiRequest.AbsoluteUrl;

			var headers = new Dictionary<string, string>(requestData.Headers);

			if (!requestData.RequiresSignature)
			{
				if (HttpMethodHelpers.HasParams(requestData.HttpMethod))
				{
					apiRequest.Parameters.Add("oauth_consumer_key", _oAuthCredentials.ConsumerKey);
				}
				else
				{
					headers.Add("Authorization", "oauth_consumer_key=" + _oAuthCredentials.ConsumerKey);
				}
			}

			if (HttpMethodHelpers.HasParams(requestData.HttpMethod) && (apiRequest.Parameters.Count > 0))
			{
				fullUrl += "?" + apiRequest.Parameters.ToQueryString();
			}

			if (requestData.RequiresSignature)
			{
				var oauthHeader = BuildOAuthHeader(requestData, fullUrl, apiRequest.Parameters);
				headers.Add("Authorization", oauthHeader);
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

			return new Request(requestData.HttpMethod, fullUrl, headers, requestBody);
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
	}
}