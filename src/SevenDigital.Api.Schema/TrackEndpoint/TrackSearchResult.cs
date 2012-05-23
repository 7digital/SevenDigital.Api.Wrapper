using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.TrackEndpoint
{
	[XmlRoot("searchResult")]
	public class TrackSearchResult
	{
		[XmlElement("type")]
		public TrackType Type { get; set; }

		[XmlElement("track")]
		public Track Track { get; set; }

		[XmlElement("score")]
		public float Score { get; set; }
	}
}