using System.Collections.Generic;
using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Requests
{
	public class RequestData
	{
		public string Endpoint { get; set; }

		public HttpMethod HttpMethod { get; set; }

		public IDictionary<string,string> Parameters { get; set; }

		public IDictionary<string,string> Headers { get; set; }

		public bool UseHttps { get; set; }

		public string OAuthToken { get; set; }

		public string OAuthTokenSecret { get; set; }

		public bool RequiresSignature { get; set; }

		public RequestPayload Payload {get; set; }

		public string Accept { get; set; }

		public IBaseUriProvider BaseUriProvider { get; set; }

		public RequestData()
		{
			Endpoint = string.Empty;
			HttpMethod = HttpMethod.Get;
			Parameters = new Dictionary<string,string>();
			Headers = new Dictionary<string,string>();
			UseHttps = false;
			Accept = "application/xml";
		}
	}
}