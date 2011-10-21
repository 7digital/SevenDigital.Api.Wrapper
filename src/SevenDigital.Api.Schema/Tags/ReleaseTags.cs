using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Tags
{
	
	[ApiEndpoint("release/tags")]
	[XmlRoot("tags")]
	public class ReleaseTags : HasPaging, HasReleaseIdParameter
	{
		[XmlElement("tag")]
		public List<Tag> TagList { get; set; }
	}
}