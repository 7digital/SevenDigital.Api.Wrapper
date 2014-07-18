using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper
{
	public interface IBaseUriProvider
	{
		string BaseUri(RequestData requestData);
	}
}