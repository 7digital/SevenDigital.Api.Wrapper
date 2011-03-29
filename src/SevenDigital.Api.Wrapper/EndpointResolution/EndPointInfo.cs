using System;
using System.Collections.Specialized;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class EndPointInfo
	{
		public string Uri { get; set; }

		public string HttpMethod { get; set; }

		public NameValueCollection Parameters { get; set; }

		public NameValueCollection Headers { get; set; }

		public bool UseHttps { get; set; }

	    public string UserToken { get; set; }

	    public string UserSecret { get; set; }

	    public EndPointInfo()
		{
			Uri = string.Empty;
			HttpMethod = "GET";
			Parameters = new NameValueCollection();
			Headers = new NameValueCollection();
			UseHttps = false;
		}
	}
}