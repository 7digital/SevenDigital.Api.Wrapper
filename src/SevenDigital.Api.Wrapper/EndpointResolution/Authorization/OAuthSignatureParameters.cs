using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	internal class OAuthSignatureParameters
	{
		public Uri Url { get;  set; }
		public string ConsumerKey { get;  set; }
		public string ConsumerSecret { get;  set; }
		public string Token { get; set; }
		public string TokenSecret { get;  set; }
		public string HttpMethod { get;  set; }
		public string TimeStamp { get;  set; }
		public string Nonce { get;  set; }
		public SignatureTypes SignatureType { get;  set; }
		public IDictionary<string, string> PostParameters { get;  set; }
		public string OAuthVersion { get;  set; }
	}
}