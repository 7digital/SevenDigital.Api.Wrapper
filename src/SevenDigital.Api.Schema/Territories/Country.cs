using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Territories
{
	[XmlRoot("country")]
	public class Country
	{
		[XmlAttribute("code")]
		public string Code { get; set; }

		[XmlElement("url")]
		public string Url { get; set; }

		[XmlElement("name")]
		public string Description { get; set; }

		[XmlElement("localName")]
		public string LocalDescription { get; set; }


		[XmlArrayItem("language")]
		[XmlArray("languages")]
		public List<Language> Languages { get; set; }
	}
}
