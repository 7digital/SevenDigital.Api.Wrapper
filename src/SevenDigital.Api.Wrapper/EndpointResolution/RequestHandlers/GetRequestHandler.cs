using System;
using System.Collections.Generic;
using OAuth;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class GetRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly ISignatureGenerator _signatureGenerator;

		public GetRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials, ISignatureGenerator signatureGenerator) : base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
			_signatureGenerator = signatureGenerator;
		}

		public override bool HandlesMethod(string method)
		{
			return method == "GET";
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

		public override string GetDebugUri(RequestData requestData)
		{
			var apiRequest = ConstructEndpoint(requestData);

			apiRequest.Parameters.Add("oauth_consumer_key", _oAuthCredentials.ConsumerKey);

			return apiRequest.FullUrl;
		}

		private GetRequest BuildGetRequest(RequestData requestData)
		{
			var uri = ConstructEndpoint(requestData);
			var signedUrl = SignHttpGetUrl(uri, requestData);
			var getRequest = new GetRequest(signedUrl, requestData.Headers);
			return getRequest;
		}

		private string SignHttpGetUrl(ApiRequest apiRequest, RequestData requestData)
		{
			if (!requestData.RequiresSignature)
			{
				apiRequest.Parameters.Add("oauth_consumer_key", _oAuthCredentials.ConsumerKey);
				return apiRequest.FullUrl;
			}

			var oauthRequest = new OAuthRequest
				{
					Type = OAuthRequestType.ProtectedResource,
					RequestUrl = apiRequest.AbsoluteUrl,
					Method = "GET",
					ConsumerKey = _oAuthCredentials.ConsumerKey,
					ConsumerSecret = _oAuthCredentials.ConsumerSecret,
				};

			if (!string.IsNullOrEmpty(requestData.UserToken))
			{
				oauthRequest.Token = requestData.UserToken;
				oauthRequest.TokenSecret = requestData.TokenSecret;
			}

			return apiRequest.AbsoluteUrl + "?" + oauthRequest.GetAuthorizationQuery(apiRequest.Parameters) +
					apiRequest.Parameters.ToQueryString(true);
		}
	}
}