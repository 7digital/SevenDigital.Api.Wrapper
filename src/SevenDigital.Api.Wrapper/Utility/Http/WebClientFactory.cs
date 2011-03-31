using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public class WebClientFactory : IWebClientFactory {
		public IWebClientWrapper GetWebClient() {
			return new WebClientWrapper(new WebClient());
		}
	}
}