using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.Playlists.Response.Endpoints
{
	[OAuthSigned]
	[ApiEndpoint("playlists")]
	[XmlRoot("playlists")]
	public class PublicPlaylists : HasPaging
	{
		[XmlElement("playlist")]
		public List<PlaylistLocation> Playlists { get; set; }
	}
}