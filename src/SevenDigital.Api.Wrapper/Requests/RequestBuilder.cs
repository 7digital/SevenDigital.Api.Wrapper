using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Requests
{
	public class RequestBuilder : IRequestBuilder
	{
		private const string FormUrlEncoded = "application/x-www-form-urlencoded";

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

			var requestBody = CheckForRequestPayload(requestData, apiRequest.Parameters);

			var oauthHeader = GetAuthorizationHeader(requestData, fullUrl, apiRequest, requestBody);
			headers.Add("Authorization", oauthHeader);
			headers.Add("Accept", requestData.Accept);
			headers.Add("x-7d-traceid", requestData.TraceId);

			if (requestData.HttpMethod.HasParamsInQueryString() && (apiRequest.Parameters.Count > 0))
			{
				fullUrl += "?" + apiRequest.Parameters.ToQueryString();
			}

			return new Request(requestData.HttpMethod, fullUrl, headers, requestBody);
		}

		private static RequestPayload CheckForRequestPayload(RequestData requestData, IDictionary<string,string> requestParameters)
		{
			var shouldHaveRequestBody = requestData.HttpMethod.ShouldHaveRequestBody();
			var hasSuppliedParameters = requestParameters.Count > 0;
			var hasSuppliedARequestPayload = requestData.Payload != null;

			if (shouldHaveRequestBody && hasSuppliedParameters)
			{
				return new RequestPayload(FormUrlEncoded, requestParameters.ToQueryString());
			}

			if (shouldHaveRequestBody && hasSuppliedARequestPayload)
			{
				return requestData.Payload;
			}

			return new RequestPayload(FormUrlEncoded, "");
		}

		private string GetAuthorizationHeader(RequestData requestData, string fullUrl, ApiRequest apiRequest, RequestPayload requestBody)
		{
			if (requestData.RequiresSignature)
			{
				return BuildOAuthHeader(requestData, fullUrl, apiRequest.Parameters, requestBody);
			}

			return "oauth_consumer_key=" + _oAuthCredentials.ConsumerKey;
		}

		private string BuildOAuthHeader(RequestData requestData, string fullUrl, IDictionary<string, string> parameters, RequestPayload requestBody)
		{
			var httpMethod = requestData.HttpMethod.ToString().ToUpperInvariant();

			var oauthRequest = new OAuthRequest
				{
					Type = OAuthRequestType.ProtectedResource,
					RequestUrl = fullUrl,
					Method = httpMethod,
					ConsumerKey = _oAuthCredentials.ConsumerKey,
					ConsumerSecret = _oAuthCredentials.ConsumerSecret,
				};

			if (!string.IsNullOrEmpty(requestData.OAuthToken))
			{
				oauthRequest.Token = requestData.OAuthToken;
				oauthRequest.TokenSecret = requestData.OAuthTokenSecret;
			}

			if (ShouldReadParamsFromBody(parameters, requestBody))
			{
				var bodyParams = HttpUtility.ParseQueryString(requestBody.Data);
				var keys = bodyParams.AllKeys.Where(x => !string.IsNullOrEmpty(x));
				foreach (var key in keys)
				{
					parameters.Add(key, bodyParams[key]);
				}
			}

			return oauthRequest.GetAuthorizationHeader(parameters);
		}

		private static bool ShouldReadParamsFromBody(IDictionary<string, string> parameters, RequestPayload requestBody)
		{
			return (requestBody.ContentType == FormUrlEncoded) && 
				!string.IsNullOrEmpty(requestBody.Data) && 
				(parameters.Count == 0);
		}
	}
}