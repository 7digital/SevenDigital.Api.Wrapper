using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Http
{
	public class RequestData
	{
		public string Endpoint { get; set; }

		public string HttpMethod { get; set; }

		public IDictionary<string,string> Parameters { get; set; }

		public IDictionary<string,string> Headers { get; set; }

		public bool UseHttps { get; set; }

		public string UserToken { get; set; }

		public string TokenSecret { get; set; }

		[Obsolete("Use TokenSecret")]
		public string UserSecret { get { return TokenSecret; } set { TokenSecret = value; } }

		public bool RequiresSignature { get; set; }

		public RequestData()
		{
			Endpoint = string.Empty;
			HttpMethod = "GET";
			Parameters = new Dictionary<string,string>();
			Headers = new Dictionary<string,string>();
			UseHttps = false;
		}
	}
}