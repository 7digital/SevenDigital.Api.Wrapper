using System;
using System.Collections.Generic;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class GetRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly IUrlSigner _urlSigner;

		public GetRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials, IUrlSigner urlSigner) : base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
			_urlSigner = urlSigner;
		}

		public override Response HitEndpoint(EndpointContext endpointContext, RequestContext requestContext)
		{
			var getRequest = BuildGetRequest(endpointContext, requestContext);
			return HttpClient.Get(getRequest);
		}

		private GetRequest BuildGetRequest(EndpointContext endpointContext, RequestContext requestContext)
		{
			var uri = ConstructEndpoint(endpointContext, requestContext);
			var signedUrl = SignHttpGetUrl(uri, endpointContext);
			var getRequest = new GetRequest(signedUrl, requestContext.Headers);
			return getRequest;
		}

		public override void HitEndpointAsync(EndpointContext endpointContext, RequestContext requestContext, Action<Response> action)
		{
			var getRequest = BuildGetRequest(endpointContext, requestContext);
			HttpClient.GetAsync(getRequest, response => action(response));
		}

		private string SignHttpGetUrl(string uri, EndpointContext endpointContext)
		{
			if (endpointContext.IsSigned)
			{
				return _urlSigner.SignGetUrl(uri, endpointContext.UserToken, endpointContext.TokenSecret, _oAuthCredentials);
			}
			return uri;
		}

		protected override string AdditionalParameters(Dictionary<string, string> newDictionary)
		{
			return string.Format("?oauth_consumer_key={0}&{1}", _oAuthCredentials.ConsumerKey, newDictionary.ToQueryString(true)).TrimEnd('&');
		}
	}
}