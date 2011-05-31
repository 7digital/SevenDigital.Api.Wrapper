using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.ReleaseEndpoint
{
	
	[ApiEndpoint("release/search")]
	[XmlRoot("searchResults")]
	public class ReleaseSearch : HasPaging
	{
		[XmlElement("searchResult")]
		public ReleaseSearchResult Results { get; set; }
	}
}