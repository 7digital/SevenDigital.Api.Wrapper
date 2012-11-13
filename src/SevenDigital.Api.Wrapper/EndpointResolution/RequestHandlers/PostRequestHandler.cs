using System;
using System.Collections.Generic;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class PostRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly IUrlSigner _urlSigner;

		public PostRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials, IUrlSigner urlSigner) : base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
			_urlSigner = urlSigner;
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
			var uri = ConstructEndpoint(requestData);
			var signedParams = SignHttpPostParams(uri, requestData);
			var postRequest = new PostRequest(uri, requestData.Headers, signedParams);
			return postRequest;
		}

		private IDictionary<string, string> SignHttpPostParams(string uri, RequestData requestData)
		{
			if (requestData.IsSigned)
			{
				return _urlSigner.SignPostRequest(uri, requestData.UserToken, requestData.TokenSecret, _oAuthCredentials, requestData.Parameters);
			}
			return requestData.Parameters;
		}

		protected override string AdditionalParameters(Dictionary<string, string> newDictionary)
		{
			return String.Empty;
		}
	}
}