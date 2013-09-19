using System.Collections.Generic;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public class OAuthSignatureInfo
	{
		public string FullUrlToSign { get; set; }
		public OAuthAccessToken UserAccessToken { get; set; }
		public IOAuthCredentials ConsumerCredentials { get; set; }
		public string HttpMethod { get; set; }
		public IDictionary<string, string> PostData { get; set; }
	}
}