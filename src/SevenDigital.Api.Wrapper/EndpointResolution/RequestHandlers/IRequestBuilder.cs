using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public interface IRequestBuilder
	{
		Request BuildRequest(RequestData requestData);
	}
}