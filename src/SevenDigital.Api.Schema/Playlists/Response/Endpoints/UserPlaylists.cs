using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.Playlists.Response.Endpoints
{
	[OAuthSigned]
	[ApiEndpoint("user/playlists")]
	[XmlRoot("playlists")]
	public class UserPlaylists : PublicPlaylists
	{}
}