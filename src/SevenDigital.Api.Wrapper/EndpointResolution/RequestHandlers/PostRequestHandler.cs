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

		public override Response HitEndpoint(EndpointContext endpointContext, RequestContext requestContext)
		{
			var postRequest = BuildPostRequest(endpointContext, requestContext);
			return HttpClient.Post(postRequest);
		}
		public override void HitEndpointAsync(EndpointContext endpointContext, RequestContext requestContext, Action<Response> action)
		{
			var postRequest = BuildPostRequest(endpointContext, requestContext);
			HttpClient.PostAsync(postRequest,response => action(response));
		}

		private PostRequest BuildPostRequest(EndpointContext endpointContext, RequestContext requestContext)
		{
			var uri = ConstructEndpoint(endpointContext, requestContext);
			var signedParams = SignHttpPostParams(uri, endpointContext, requestContext);
			var postRequest = new PostRequest(uri, requestContext.Headers, signedParams);
			return postRequest;
		}

		private IDictionary<string, string> SignHttpPostParams(string uri, EndpointContext endpointContext, RequestContext requestContext)
		{
			if (endpointContext.IsSigned)
			{
				return _urlSigner.SignPostRequest(uri, endpointContext.UserToken, endpointContext.TokenSecret, _oAuthCredentials, requestContext.Parameters);
			}
			return requestContext.Parameters;
		}

		protected override string AdditionalParameters(Dictionary<string, string> newDictionary)
		{
			return String.Empty;
		}
	}
}