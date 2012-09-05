using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.OAuth
{
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