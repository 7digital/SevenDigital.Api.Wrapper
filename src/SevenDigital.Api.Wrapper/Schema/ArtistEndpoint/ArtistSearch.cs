using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.ArtistEndpoint
{
	[Serializable]
	[ApiEndpoint("artist/search")]
	[XmlRoot("searchResults")]
	public class ArtistSearch : HasPaging
	{
		[XmlElement("searchResult")]
		public ArtistSearchResult Results { get; set; }
	}
}