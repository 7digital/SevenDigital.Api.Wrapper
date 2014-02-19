using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.Playlists.Response.Endpoints
{
	[OAuthSigned]
	[ApiEndpoint("playlists/{playlistId}")]
	[XmlRoot("playlist")]
	public class Playlist : PlaylistDetails
	{
		[XmlArray("tracks")]
		[XmlArrayItem("track")]
		public List<PlaylistProduct> Tracks { get; set; }
	}
}