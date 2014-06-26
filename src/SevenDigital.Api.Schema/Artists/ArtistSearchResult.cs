using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Artists
{
	[XmlRoot("searchResult")]
	public class ArtistSearchResult
	{
		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("artist")]
		public Artist Artist { get; set; }

		[XmlElement("score")]
		public float Score { get; set; }
	}
}