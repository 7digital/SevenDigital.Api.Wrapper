using System;

namespace SevenDigital.Api.Wrapper.Http
{
	public class EndpointContext
	{
		public string UriPath { get; set; }
		public string HttpMethod { get; set; }
		public bool UseHttps { get; set; }
		public string UserToken { get; set; }
		public string TokenSecret { get; set; }
		[Obsolete("Use TokenSecret")]
		public string UserSecret { get { return TokenSecret; } set { TokenSecret = value; } }
		public bool IsSigned { get; set; }

		public EndpointContext()
		{
			UriPath = string.Empty;
			HttpMethod = "GET";
		}
	}
}