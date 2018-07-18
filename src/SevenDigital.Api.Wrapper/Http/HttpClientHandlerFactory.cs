using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Http
{
	public class HttpClientHandlerFactory : IHttpClientHandlerFactory
	{
		public HttpClientHandler CreateHandler()
		{
			return new HttpClientHandler();
		}
	}
}
