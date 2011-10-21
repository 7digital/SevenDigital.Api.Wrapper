using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Tags
{
	[ApiEndpoint("tag/releated")]
	[XmlRoot("tags")]
	public class ReleatedTags : HasPaging
	{
		[XmlElement("tag")]
		public List<Tag> TagList { get; set; }
	}
}
