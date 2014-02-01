namespace SevenDigital.Api.Wrapper.Http
{
	public interface IHttpClient
	{
		Response Send(Request request);
	}
}