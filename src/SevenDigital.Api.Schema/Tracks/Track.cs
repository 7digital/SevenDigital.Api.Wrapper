using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.Media;
using SevenDigital.Api.Schema.Package;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.Pricing;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Schema.Tracks
{
	[XmlRoot("track")]
	[ApiEndpoint("track/details")]
	[Serializable]
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

		/// <summary>
		/// Track number or in cases where disc number is greater than 1, it is the discNumber + trackNumber (ie 203)
		/// </summary>
		/// <remarks>Will soon de decommisioned</remarks>
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

		[XmlElement("image")]
		public string Image { get; set; }

		[XmlElement("price")]
		public Price Price { get; set; }

		[XmlElement("type")]
		public TrackType Type { get; set; }

		[XmlElement(ElementName = "streamingReleaseDate", IsNullable = true)]
		public DateTime? StreamingReleaseDate { get; set; }

		[XmlElement("discNumber")]
		public int DiscNumber { get; set; }

		[XmlElement("formats")]
		public FormatList Formats { get; set; }

		/// <summary>
		/// Track Number. Should be used instead of "TrackNumber"
		/// </summary>
		[XmlElement("number")]
		public int Number { get; set; }

        [XmlElement("download")]
        public PackageList Download { get; set; }
	}
}