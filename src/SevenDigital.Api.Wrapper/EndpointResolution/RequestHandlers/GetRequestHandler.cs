using System;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;
using OAuth;

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

		public override void HitEndpointAsync(RequestData requestData, Action<Response> action)
		{
			var getRequest = BuildGetRequest(requestData);
			HttpClient.GetAsync(getRequest, response => action(response));
		}

		private GetRequest BuildGetRequest(RequestData requestData)
		{
			var apiRequest = MakeApiRequest(requestData);
			if (!requestData.RequiresSignature)
			{
				apiRequest.Parameters.Add("oauth_consumer_key", _oAuthCredentials.ConsumerKey);
			}

			if (apiRequest.Parameters.Count > 0)
			{
				apiRequest.AbsoluteUrl = apiRequest.AbsoluteUrl + "?" + apiRequest.Parameters.ToQueryString();
			}
			AddOAuthHeader(requestData, apiRequest);

			return new GetRequest(apiRequest.AbsoluteUrl, requestData.Headers);
		}

		private void AddOAuthHeader(RequestData requestData, ApiRequest apiRequest)
		{
			var oauthHeaderHGenerator = new OAuthHeaderGenerator(_oAuthCredentials);
			var oauthHeader = oauthHeaderHGenerator.GenerateOAuthSignatureHeader(apiRequest, requestData);
			requestData.Headers.Add("Authorization", oauthHeader);
		}

		private string SignHttpGetUrl(ApiRequest apiRequest, RequestData requestData)
		{
			if (!requestData.RequiresSignature)
			{
				apiRequest.Parameters.Add("oauth_consumer_key", _oAuthCredentials.ConsumerKey);
				return apiRequest.FullUri;
			}

			var oauthRequest = new OAuthRequest
				{
					Type = OAuthRequestType.ProtectedResource,
					RequestUrl = apiRequest.AbsoluteUrl,
					Method = "GET",
					ConsumerKey = _oAuthCredentials.ConsumerKey,
					ConsumerSecret = _oAuthCredentials.ConsumerSecret
				};

			AddTokenIfRequired(oauthRequest, requestData);

			return apiRequest.AbsoluteUrl 
				+ "?" 
				+ oauthRequest.GetAuthorizationQuery(apiRequest.Parameters) 
				+ apiRequest.Parameters.ToQueryString();
		}

		private void AddTokenIfRequired(OAuthRequest oauthRequest, RequestData requestData)
		{
			if (requestData.HasToken)
			{
				oauthRequest.Token = requestData.UserToken;
				oauthRequest.TokenSecret = requestData.TokenSecret;
			}
		}

		public override string GetDebugUri(RequestData requestData)
		{
			var apiRequest = MakeApiRequest(requestData);
			apiRequest.Parameters.Add("oauth_consumer_key", _oAuthCredentials.ConsumerKey);
			return apiRequest.FullUri;
		}
	}
}