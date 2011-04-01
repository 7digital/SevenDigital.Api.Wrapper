namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IWebClientFactory {
		IWebClientWrapper GetWebClient();
	}
}