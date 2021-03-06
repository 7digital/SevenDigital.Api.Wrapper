﻿using OAuth;
using SevenDigital.Api.Wrapper.Http;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenDigital.Api.Wrapper.Requests
{
	public class RequestBuilder : IRequestBuilder
	{
		private const string FormUrlEncoded = "application/x-www-form-urlencoded";

		// API router will allow these params regardless of HTTP verb (https://git.io/fNZZi)
		private readonly string[] _allowedQueryStringParams = { "userid", "country", "shopid", "onbehalfofpartnerid" };

		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly IRouteParamsSubstitutor _routeParamsSubstitutor;

		public RequestBuilder(IRouteParamsSubstitutor routeParamsSubstitutor, IOAuthCredentials oAuthCredentials)
		{
			_oAuthCredentials = oAuthCredentials;
			_routeParamsSubstitutor = routeParamsSubstitutor;
		}

		private IDictionary<string, string> GetAllowedQueryStringParams(IDictionary<string, string> parameters)
		{
			return parameters.Where(kv => _allowedQueryStringParams.Contains(kv.Key.ToLower()))
				.ToDictionary(kv => kv.Key, i => i.Value);
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

			if (requestData.TraceId != null)
			{
				headers.Add("x-7d-traceid", requestData.TraceId);
			}

			var queryParams = requestData.HttpMethod.ShouldHaveRequestBody()
				? GetAllowedQueryStringParams(apiRequest.Parameters)
				: apiRequest.Parameters;

			if (queryParams.Any())
			{
				fullUrl += "?" + queryParams.ToQueryString();
			}

			return new Request(requestData.HttpMethod, fullUrl, headers, requestBody, requestData.TraceId);
		}

		private static RequestPayload CheckForRequestPayload(RequestData requestData, IDictionary<string,string> requestParameters)
		{
			var shouldHaveRequestBody = requestData.HttpMethod.ShouldHaveRequestBody();
			var hasSuppliedARequestPayload = requestData.Payload != null;

			if (shouldHaveRequestBody && hasSuppliedARequestPayload)
			{
				return requestData.Payload;
			}

			return new RequestPayload(FormUrlEncoded, requestParameters.ToQueryString());
		}

		private string GetAuthorizationHeader(RequestData requestData, string fullUrl, ApiRequest apiRequest, RequestPayload requestBody)
		{
			if (requestData.RequiresSignature)
			{
				return BuildOAuthHeader(requestData, fullUrl, apiRequest.Parameters, requestBody);
			}

			return "oauth_consumer_key=" + _oAuthCredentials.ConsumerKey;
		}

		private string BuildOAuthHeader(RequestData requestData, string fullUrl, IDictionary<string, string> queryStringParameters, RequestPayload requestBody)
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

			var headerParameters = queryStringParameters.ToDictionary(x => x.Key, x => x.Value);

			if (ShouldReadParamsFromBody(queryStringParameters, requestBody))
			{
				var bodyParams = HttpUtility.ParseQueryString(requestBody.Data);
				var keys = bodyParams.AllKeys.Where(x => !string.IsNullOrEmpty(x));
				foreach (var key in keys)
				{
					headerParameters.Add(key, bodyParams[key]);
				}
			}

			return oauthRequest.GetAuthorizationHeader(headerParameters);
		}

		private static bool ShouldReadParamsFromBody(IDictionary<string, string> queryStringParameters, RequestPayload requestBody)
		{
			return (requestBody.ContentType == FormUrlEncoded) && 
				!string.IsNullOrEmpty(requestBody.Data) && 
				(queryStringParameters.Count == 0);
		}
	}
}