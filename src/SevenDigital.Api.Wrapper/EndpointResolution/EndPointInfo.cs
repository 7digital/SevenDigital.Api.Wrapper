using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class EndPointInfo
	{
		public string Uri { get; set; }

		public string HttpMethod { get; set; }

        public Dictionary<string,string> Parameters { get; set; }

		public Dictionary<string,string> Headers { get; set; }

		public bool UseHttps { get; set; }

	    public string UserToken { get; set; }

	    public string UserSecret { get; set; }

		public bool IsSigned { get; set; }

	    public EndPointInfo()
		{
			Uri = string.Empty;
			HttpMethod = "GET";
            Parameters = new Dictionary<string,string>();
            Headers = new Dictionary<string,string>();
			UseHttps = false;
		}
	}
}