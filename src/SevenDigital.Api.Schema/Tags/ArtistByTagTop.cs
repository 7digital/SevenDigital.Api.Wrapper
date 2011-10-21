using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Tags
{

	[ApiEndpoint("artist/bytag/top")]
	[XmlRoot("taggedResults")]
	public class ArtistByTagTop : HasPaging
	{
		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("taggedItem")]
		public List<TaggedArtists> TaggedArtists { get; set; }
	}
}