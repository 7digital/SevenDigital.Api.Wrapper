using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public interface IRequestHandler
	{
		IHttpClient HttpClient { get; set; }
		Response HitEndpoint(RequestData requestData);
		string GetDebugUri(RequestData requestData);
	}
}