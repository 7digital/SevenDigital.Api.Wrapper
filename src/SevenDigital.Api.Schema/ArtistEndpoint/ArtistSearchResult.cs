using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.ArtistEndpoint
{
	[Serializable]
	[XmlRoot("searchResult")]
	public class ArtistSearchResult
	{
		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("artist")]
		public List<Artist> Artists { get; set; }
	}
}