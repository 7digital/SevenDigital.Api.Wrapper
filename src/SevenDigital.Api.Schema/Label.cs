using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema
{
	
	[XmlRoot("label")]
	[Serializable]
	public class Label
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }
	}
}