using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Playlists.Response
{
	[XmlRoot("track")]
	public class PlaylistProduct : Product
	{
		[XmlAttribute("id")]
		public string PlaylistItemId { get; set; }

		[XmlElement("user", IsNullable = true)]
		public string User { get; set; }

		[XmlElement("dateAdded", IsNullable = true)]
		public DateTime? DateAdded { get; set; }
	}
}