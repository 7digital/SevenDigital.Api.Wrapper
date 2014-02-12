namespace SevenDigital.Api.Wrapper.Requests
{
	public interface IRequestBuilder
	{
		Request BuildRequest(RequestData requestData);
	}
}