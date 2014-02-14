using System.Collections.Generic;
using OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Requests
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

			if (HttpMethodHelpers.HasParamsInQueryString(requestData.HttpMethod) && (apiRequest.Parameters.Count > 0))
			{
				fullUrl += "?" + apiRequest.Parameters.ToQueryString();
			}

			RequestPayload requestBody = null;
			if (HttpMethodHelpers.ShouldHaveRequestBody(requestData.HttpMethod) && (apiRequest.Parameters.Count > 0))
			{
				requestBody = new RequestPayload("application/x-www-form-urlencoded", apiRequest.Parameters.ToQueryString());
			}
			else if (HttpMethodHelpers.ShouldHaveRequestBody(requestData.HttpMethod) && requestData.Payload != null)
			{
				requestBody = requestData.Payload;
			}

			return new Request(requestData.HttpMethod, fullUrl, headers, requestBody);
		}

		private string GetAuthorizationHeader(RequestData requestData, string fullUrl, ApiRequest apiRequest)
		{
			if (requestData.RequiresSignature)
			{
				return BuildOAuthHeader(requestData, fullUrl, apiRequest.Parameters);
			}

			return "oauth_consumer_key=" + _oAuthCredentials.ConsumerKey;
		}

		private string BuildOAuthHeader(RequestData requestData, string fullUrl, IDictionary<string, string> parameters)
		{
			var oauthRequest = new OAuthRequest
				{
					Type = OAuthRequestType.ProtectedResource,
					RequestUrl = fullUrl,
					Method = requestData.HttpMethod.ToString().ToUpperInvariant(),
					ConsumerKey = _oAuthCredentials.ConsumerKey,
					ConsumerSecret = _oAuthCredentials.ConsumerSecret,
				};

			if (!string.IsNullOrEmpty(requestData.UserToken))
			{
				oauthRequest.Token = requestData.UserToken;
				oauthRequest.TokenSecret = requestData.TokenSecret;
			}

			return oauthRequest.GetAuthorizationHeader(parameters);
		}
	}
}