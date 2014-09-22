using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Playlists
{
	[XmlRoot("track")]
	public class Product
	{
		[XmlElement("trackId")]
		public string TrackId { get; set; }

		[XmlElement("trackTitle", IsNullable = true)]
		public string TrackTitle { get; set; }

		[XmlElement("trackVersion", IsNullable = true)]
		public string TrackVersion { get; set; }

		[XmlElement("artistId", IsNullable = true)]
		public string ArtistId { get; set; }

		[XmlElement("artistAppearsAs")]
		public string ArtistAppearsAs { get; set; }

		[XmlElement("releaseId", IsNullable = true)]
		public string ReleaseId { get; set; }

		[XmlElement("releaseTitle", IsNullable = true)]
		public string ReleaseTitle { get; set; }

		[XmlElement("releaseArtistId", IsNullable = true)]
		public string ReleaseArtistId { get; set; }

		[XmlElement("releaseArtistAppearsAs", IsNullable = true)]
		public string ReleaseArtistAppearsAs { get; set; }

		[XmlElement("releaseVersion", IsNullable = true)]
		public string ReleaseVersion { get; set; }

		[XmlElement("source", IsNullable = true)]
		public string Source { get; set; }

		[XmlElement("audioUrl", IsNullable = true)]
		public string AudioUrl { get; set; }

		[XmlElement("image", IsNullable = true)]
		public string ImageUrl { get; set; }
	}
}