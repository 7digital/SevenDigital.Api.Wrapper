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

		private PostRequest BuildPostRequest(RequestData requestData)
		{
			var apiRequest = MakeApiRequest(requestData);
			var requestBody = SignHttpPostParams(apiRequest.AbsoluteUrl, requestData);
			var postRequest = new PostRequest(apiRequest.AbsoluteUrl, requestData.Headers, requestBody);
			return postRequest;
		}

		private string SignHttpPostParams(string uri, RequestData requestData)
		{
			if (!requestData.RequiresSignature)
			{
				var @params =  new Dictionary<string, string>(requestData.Parameters)
					{
						{"oauth_consumer_key", _oAuthCredentials.ConsumerKey}
					};

				return @params.ToQueryString();
			}

			var oAuthSignatureInfo = new OAuthSignatureInfo
			{
				FullUrlToSign = uri,
				ConsumerCredentials = _oAuthCredentials,
				HttpMethod = "POST",
				UserAccessToken = new OAuthAccessToken { Token = requestData.UserToken, Secret = requestData.TokenSecret },
				PostData = requestData.Parameters
			};

			return _signatureGenerator.SignWithPostData(oAuthSignatureInfo).ToQueryStringNoUrlEncode();
		}

		public override string GetDebugUri(RequestData requestData)
		{
			return MakeApiRequest(requestData).FullUri;
		}
	}
}