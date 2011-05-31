using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Tags
{
	
	[ApiEndpoint("tag")]
	[XmlRoot("tags")]
	public class Tags : HasPaging
	{
		[XmlElement("tag")]
		public List<Tag> TagList { get; set; }
	}
}