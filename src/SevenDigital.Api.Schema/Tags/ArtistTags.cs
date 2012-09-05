using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Tags
{
	[ApiEndpoint("artist/tags")]
	[XmlRoot("tags")]
	public class ArtistTags : HasPaging
	{
		[XmlElement("tag")]
		public List<Tag> TagList { get; set; }
	}
}