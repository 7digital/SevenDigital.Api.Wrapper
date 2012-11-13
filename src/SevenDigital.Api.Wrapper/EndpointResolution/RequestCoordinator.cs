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

		public string ConstructEndpoint(RequestData requestData)
		{
			return ConstructBuilder(requestData).ConstructEndpoint(requestData);
		}

		private RequestHandler ConstructBuilder(RequestData requestData)
		{
			switch (requestData.HttpMethod.ToUpperInvariant())
			{
				case "GET":
					return new GetRequestHandler(_apiUri, _oAuthCredentials, _urlSigner);
				case "POST":
					return new PostRequestHandler(_apiUri, _oAuthCredentials, _urlSigner);
				default:
					throw new NotImplementedException();
			}
		}

		public virtual Response HitEndpoint(RequestData requestData)
		{
			var builder = ConstructBuilder(requestData);
			builder.HttpClient = HttpClient;
			return builder.HitEndpoint(requestData);
		}

		public virtual void HitEndpointAsync(RequestData requestData, Action<Response> callback)
		{
			var builder = ConstructBuilder(requestData);
			builder.HttpClient = HttpClient;
			builder.HitEndpointAsync(requestData, callback);
		}
	}
}