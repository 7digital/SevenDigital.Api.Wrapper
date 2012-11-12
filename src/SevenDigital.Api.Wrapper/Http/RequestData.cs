using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Http
{
	public class RequestData
	{
		public string UriPath { get; set; }

		public string HttpMethod { get; set; }

		public Dictionary<string,string> Parameters { get; set; }

		public Dictionary<string,string> Headers { get; set; }

		public bool UseHttps { get; set; }

		public string UserToken { get; set; }

		public string TokenSecret { get; set; }

		[Obsolete("Use TokenSecret")]
		public string UserSecret { get { return TokenSecret; } set { TokenSecret = value; } }

		public bool IsSigned { get; set; }

		public RequestData()
		{
			UriPath = string.Empty;
			HttpMethod = "GET";
			Parameters = new Dictionary<string,string>();
			Headers = new Dictionary<string,string>();
			UseHttps = false;
		}
	}
}