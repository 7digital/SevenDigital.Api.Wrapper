using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Playlists.Response
{
	[XmlRoot("playlist")]
	public class PlaylistLocation : UserBasedUpdatableItem
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlArray("links")]
		[XmlArrayItem("link")]
		public List<Link> Links { get; set; }

		[XmlElement("trackCount")]
		public int TrackCount { get; set; }

		[XmlElement("visibility")]
		public PlaylistVisibilityType Visibility { get; set; }
	}
}