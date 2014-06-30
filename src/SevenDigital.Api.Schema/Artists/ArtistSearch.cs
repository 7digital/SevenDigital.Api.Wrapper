using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Artists
{
	[ApiEndpoint("artist/search")]
	[XmlRoot("searchResults")]
	public class ArtistSearch : HasPaging, HasSearchParameter
	{
		public ArtistSearch()
		{
			Results = new List<ArtistSearchResult>();
		}

		[XmlElement("searchResult")]
		public List<ArtistSearchResult> Results { get; set; }
	}
}