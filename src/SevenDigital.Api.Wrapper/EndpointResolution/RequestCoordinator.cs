using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class RequestCoordinator : IRequestCoordinator
	{
		private readonly AllRequestHandler _requestHandler;

		public IHttpClient HttpClient { get; set; }

		public RequestCoordinator(IHttpClient httpClient, AllRequestHandler requestHandler)
		{
			HttpClient = httpClient;
			_requestHandler = requestHandler;
		}

		public string ConstructEndpoint(RequestData requestData)
		{
			return _requestHandler.GetDebugUri(requestData);
		}

		public virtual Response HitEndpoint(RequestData requestData)
		{
			_requestHandler.HttpClient = HttpClient;
			return _requestHandler.HitEndpoint(requestData);
		}
	}
}