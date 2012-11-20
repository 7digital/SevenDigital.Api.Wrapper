using System;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class RequestCoordinator : IRequestCoordinator
	{
		private readonly IUrlSigner _urlSigner;
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly IApiUri _apiUri;
		
		public IHttpClient HttpClient { get; set; }

		public RequestCoordinator(IHttpClient httpClient, IUrlSigner urlSigner, IOAuthCredentials oAuthCredentials, IApiUri apiUri)
		{
			HttpClient = httpClient;
			_urlSigner = urlSigner;
			_oAuthCredentials = oAuthCredentials;
			_apiUri = apiUri;
		}

		public string ConstructEndpoint(EndpointContext endpointContext, RequestContext requestContext)
		{
			return ConstructBuilder(endpointContext).ConstructEndpoint(endpointContext, requestContext);
		}

		private RequestHandler ConstructBuilder(EndpointContext endpointContext)
		{
			switch (endpointContext.HttpMethod.ToUpperInvariant())
			{
				case "GET":
					return new GetRequestHandler(_apiUri, _oAuthCredentials, _urlSigner);
				case "POST":
					return new PostRequestHandler(_apiUri, _oAuthCredentials, _urlSigner);
				default:
					throw new NotImplementedException();
			}
		}

		public virtual Response HitEndpoint(EndpointContext endpointContext, RequestContext requestContext)
		{
			var builder = ConstructBuilder(endpointContext);
			builder.HttpClient = HttpClient;
			return builder.HitEndpoint(endpointContext, requestContext);
		}

		public virtual void HitEndpointAsync(EndpointContext endpointContext, RequestContext requestContext, Action<Response> callback)
		{
			var builder = ConstructBuilder(endpointContext);
			builder.HttpClient = HttpClient;
			builder.HitEndpointAsync(endpointContext, requestContext, callback);
		}
	}
}