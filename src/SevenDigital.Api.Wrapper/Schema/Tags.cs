using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema {

	[Serializable]
	[ApiEndpoint("tag")]
	[XmlRoot("tags")]
	public class Tags : HasPaging {
		[XmlElement("tag")]
		public List<Tag> TagList { get; set; }
	}

	[Serializable]
	[XmlRoot("tag")]
	public class Tag
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("text")]
		public string Text { get; set; }

		[XmlElement("url")]
		public string Url { get; set; }

		[XmlElement("count")]
		public int Count { get; set; }
	}
}
