using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema
{
	[XmlRoot("licensor")]
	[Serializable]
	public class Licensor
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }
	}
}