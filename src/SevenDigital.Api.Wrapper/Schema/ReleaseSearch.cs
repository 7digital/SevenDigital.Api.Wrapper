using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema
{
	[Serializable]
	[ApiEndpoint("release/search")]
	[XmlRoot("searchResults")]
	public class ReleaseSearch
	{
		[XmlElement("page")]
		public int Page { get; set; }

		[XmlElement("pageSize")]
		public int PageSize { get; set; }

		[XmlElement("totalItems")]
		public int TotalItems { get; set; }

		[XmlElement("searchResult")]
		public ReleaseSearchResult Results { get; set; }
	}
}