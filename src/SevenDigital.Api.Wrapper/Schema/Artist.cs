using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Schema
{
	[Serializable]
	[XmlRoot("artist")]
	public class Artist
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("sortName")]
		public string SortName { get; set; }

		[XmlElement("appearsAs")]
		public string AppearsAs { get; set; }

		[XmlElement("image")]
		public string Image { get; set; }

		[XmlElement("url")]
		public string Url { get; set; }
	}
}
