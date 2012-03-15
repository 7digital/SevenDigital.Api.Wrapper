using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.Pricing;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Schema.User.Purchase;

namespace SevenDigital.Api.Schema.TrackEndpoint
{
	
	[XmlRoot("track")]
	[ApiEndpoint("track/details")]
	public class Track : HasTrackIdParameter
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("title")]
		public string Title { get; set; }

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

		[XmlElement("type")]
		public ReleaseType Type { get; set; }
	}
}