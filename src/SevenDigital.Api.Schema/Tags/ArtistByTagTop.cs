using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Tags
{
	[ApiEndpoint("artist/bytag/top")]
	[XmlRoot("taggedResults")]
	public class ArtistByTagTop : HasPaging, HasTags
	{
		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("taggedItem")]
		public List<TaggedArtist> TaggedArtists { get; set; }
	}
}