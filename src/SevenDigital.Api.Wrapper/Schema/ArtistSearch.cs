using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema
{
	[Serializable]
	[ApiEndpoint("artist/search")]
	[XmlRoot("searchResults")]
	public class ArtistSearch
	{
		[XmlElement("page")]
		public int Page { get; set; }

		[XmlElement("pageSize")]
		public int PageSize { get; set; }

		[XmlElement("totalItems")]
		public int TotalItems { get; set; }

		[XmlElement("searchResult")]
		public ArtistSearchResult Results { get; set; }
	}
}