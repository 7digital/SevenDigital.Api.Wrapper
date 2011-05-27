using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.OAuth
{
	[Serializable]
	[ApiEndpoint("oauth/accesstoken")]
	[XmlRoot("oauth_access_token")]
	[OAuthSigned]
	public class OAuthAccessToken
	{
		[XmlElement("oauth_token")]
		public string Token { get; set; }

		[XmlElement("oauth_token_secret")]
		public string Secret { get; set; }
	}
}