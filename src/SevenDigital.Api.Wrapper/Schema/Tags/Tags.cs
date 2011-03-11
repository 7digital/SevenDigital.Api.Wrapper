using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.Tags
{
	[Serializable]
	[ApiEndpoint("tag")]
	[XmlRoot("tags")]
	public class Tags : HasPaging {
		[XmlElement("tag")]
		public List<Tag> TagList { get; set; }
	}
}