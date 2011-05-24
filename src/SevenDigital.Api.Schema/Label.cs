using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema
{
	[Serializable]
	[XmlRoot("label")]
	public class Label
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }
	}
}