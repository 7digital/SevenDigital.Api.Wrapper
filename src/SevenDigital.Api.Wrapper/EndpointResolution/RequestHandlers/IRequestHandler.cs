using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public interface IRequestHandler
	{
		Request BuildRequest(RequestData requestData);
	}
}