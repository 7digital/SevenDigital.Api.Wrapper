using System.Collections.Generic;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class RequestBuilder : IRequestBuilder
	{
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly RouteParamsSubstitutor _routeParamsSubstitutor;

		public RequestBuilder(IApiUri apiUri, IOAuthCredentials oAuthCredentials)
		{
			_oAuthCredentials = oAuthCredentials;
			_routeParamsSubstitutor = new RouteParamsSubstitutor(apiUri);
		}

		public Request BuildRequest(RequestData requestData)
		{
			var apiRequest = _routeParamsSubstitutor.SubstituteParamsInRequest(requestData);
			var fullUrl = apiRequest.AbsoluteUrl;

			var headers = new Dictionary<string, string>(requestData.Headers);

			 var oauthHeader = GetAuthorizationHeader(requestData, fullUrl, apiRequest);
			headers.Add("Authorization", oauthHeader);

			if (HttpMethodHelpers.HasParams(requestData.HttpMethod) && (apiRequest.Parameters.Count > 0))
			{
				fullUrl += "?" + apiRequest.Parameters.ToQueryString();
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

		private string GetAuthorizationHeader(RequestData requestData, string fullUrl, ApiRequest apiRequest)
		{
			if (requestData.RequiresSignature)
			{
				return BuildOAuthHeader(requestData, fullUrl, apiRequest.Parameters);
			}

			return _oAuthCredentials.ConsumerKey;
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