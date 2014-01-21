using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public class OAuthHeaderData
	{
		public string Url { get; set; }
		public HttpMethod HttpMethod { get; set; }
		public string UserToken { get; set; }
		public string TokenSecret { get; set; }
	}
}