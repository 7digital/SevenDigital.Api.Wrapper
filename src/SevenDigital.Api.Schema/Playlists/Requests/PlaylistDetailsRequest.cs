using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Playlists.Requests
{
	[XmlRoot("playlist")]
	public class PlaylistDetailsRequest
	{
		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("visibility")]
		public PlaylistVisibilityType Visibility { get; set; }
	}
}