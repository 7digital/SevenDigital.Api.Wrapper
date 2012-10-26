using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Territories
{
	[Serializable]
	public class Language
	{
		[XmlElement("name")]
		public string Description { get; set; }

		[XmlElement("localName")]
		public string LocalDescription { get; set; }

		[XmlElement("url")]
		public string Url { get; set; }
	}
}
