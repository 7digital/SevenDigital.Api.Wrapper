using System;
using System.Collections.Generic;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class PostRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly ISignatureGenerator _signatureGenerator;

		public PostRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials, ISignatureGenerator signatureGenerator)
			: base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
			_signatureGenerator = signatureGenerator;
		}

		public override bool HandlesMethod(string method)
		{
			return method == "POST";
		}

		public override Response HitEndpoint(RequestData requestData)
		{
			var postRequest = BuildPostRequest(requestData);
			return HttpClient.Post(postRequest);
		}

		public override void HitEndpointAsync(RequestData requestData, Action<Response> action)
		{
			var postRequest = BuildPostRequest(requestData);
			HttpClient.PostAsync(postRequest,response => action(response));
		}

		public override string GetDebugUri(RequestData requestData)
		{
			var apiRequest = ConstructEndpoint(requestData);
			return apiRequest.AbsoluteUrl;
		}

		private PostRequest BuildPostRequest(RequestData requestData)
		{
			var apiRequest = ConstructEndpoint(requestData);
			var signedParams = SignHttpPostParams(apiRequest.AbsoluteUrl, requestData);
			var postRequest = new PostRequest(apiRequest.AbsoluteUrl, requestData.Headers, signedParams);
			return postRequest;
		}

		private IDictionary<string, string> SignHttpPostParams(string uri, RequestData requestData)
		{
			if (!requestData.RequiresSignature)
			{
				return requestData.Parameters;
			}
			var oAuthSignatureInfo = new OAuthSignatureInfo
			{
				FullUrlToSign = uri,
				ConsumerCredentials = _oAuthCredentials,
				HttpMethod = "POST",
				UserAccessToken = new OAuthAccessToken { Token = requestData.UserToken, Secret = requestData.TokenSecret },
				PostData = requestData.Parameters
			};

			return _signatureGenerator.SignWithPostData(oAuthSignatureInfo);
		}
	}
}