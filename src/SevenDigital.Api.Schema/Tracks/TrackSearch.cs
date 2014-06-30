using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Tracks
{
	[XmlRoot("searchResults")]
	[ApiEndpoint("track/search")]
	public class TrackSearch : HasPaging, HasSearchParameter
	{
		public TrackSearch()
		{
			Results = new List<TrackSearchResult>();
		}

		[XmlElement("searchResult")]
		public List<TrackSearchResult> Results { get; set; }
	}
}