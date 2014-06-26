using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Releases
{
	[ApiEndpoint("release/search")]
	[XmlRoot("searchResults")]
	public class ReleaseSearch : HasPaging, HasSearchParameter
	{
		public ReleaseSearch()
		{
			Results = new List<ReleaseSearchResult>();
		}

		[XmlElement("searchResult")]
		public List<ReleaseSearchResult> Results { get; set; }
	}
}