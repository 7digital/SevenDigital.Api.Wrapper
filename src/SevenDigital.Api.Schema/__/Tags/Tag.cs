using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Tags
{
	
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