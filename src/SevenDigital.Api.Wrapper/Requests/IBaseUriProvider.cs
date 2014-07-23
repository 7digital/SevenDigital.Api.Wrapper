namespace SevenDigital.Api.Wrapper.Requests
{
	public interface IBaseUriProvider
	{
		string BaseUri(RequestData requestData);
	}
}