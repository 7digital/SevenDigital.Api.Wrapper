using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Schema
{
	[Serializable]
	[XmlRoot("track")]
	public class Track
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("version")]
		public string Version { get; set; }

		[XmlElement("artist")]
		public Artist Artist { get; set; }

		[XmlElement("trackNumber")]
		public int TrackNumber { get; set; }

		[XmlElement("duration")]
		public int Duration { get; set; }

		[XmlElement("explicitContent")]
		public bool ExplicitContent { get; set; }

		[XmlElement("isrc")]
		public string Isrc { get; set; }

		[XmlElement("release")]
		public Release Release { get; set; }

		[XmlElement("url")]
		public string Url { get; set; }

		[XmlElement("price")]
		public Price Price { get; set; }
	}
}