using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.ArtistEndpoint
{
	[ApiEndpoint("artist/search")]
	[XmlRoot("searchResults")]
	public class ArtistSearch : HasPaging
	{
		[XmlElement("searchResult")]
		public ArtistSearchResult Results { get; set; }
	}
}