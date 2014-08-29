using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Artists
{
	[ApiEndpoint("artist/similar")]
	[XmlRoot("artists")]
	public class ArtistSimilar : HasPaging, HasArtistIdParameter
	{
		[XmlElement("artist")]
		public List<Artist> Artists { get; set; }
	}
}
