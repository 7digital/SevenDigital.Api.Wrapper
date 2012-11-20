
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper
{
	public interface IApiEndpoint
	{
		string EndpointUrl(RequestContext requestContext);
	}
}
