using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.TrackEndpoint
{
	[Serializable]
	[XmlRoot("searchResults")]
	[ApiEndpoint("track/search")]
	public class TrackSearch : HasPaging
	{
		[XmlElement("searchResult")]
		public List<TrackSearchResult> Results { get; set; }
	}
}