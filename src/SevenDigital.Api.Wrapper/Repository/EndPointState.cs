using System.Collections.Specialized;

namespace SevenDigital.Api.Wrapper.Repository
{
	public class EndPointState
	{
		public string Uri { get; set; }

		public string HttpMethod { get; set; }

		public NameValueCollection Parameters { get; set; }

		public NameValueCollection Headers { get; set; }

		public EndPointState()
		{
			Uri = string.Empty;
			HttpMethod = "GET";
			Parameters = new NameValueCollection();
			Headers = new NameValueCollection();
		}
	}
}