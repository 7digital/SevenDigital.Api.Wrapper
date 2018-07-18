using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Http
{
	public interface IHttpClientHandlerFactory
	{
		HttpClientHandler CreateHandler();
	}
}
