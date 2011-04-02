using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.OAuth
{
	[Serializable]
	[ApiEndpoint("oauth/requesttoken")]
	[XmlRoot("oauth_request_token")]
	[OAuthSigned]
	public class OAuthRequestToken
	{
		[XmlElement("oauth_token")]
		public string Token { get; set; }

		[XmlElement("oauth_token_secret")]
		public string Secret { get; set; }
	}
}
