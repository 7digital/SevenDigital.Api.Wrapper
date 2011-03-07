using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint
{
	[Serializable]
	[ApiEndpoint("release/search")]
	[XmlRoot("searchResults")]
	public class ReleaseSearch : HasPaging
	{
		[XmlElement("searchResult")]
		public ReleaseSearchResult Results { get; set; }
	}
}