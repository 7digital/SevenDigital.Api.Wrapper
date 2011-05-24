using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Media
{
	[Serializable]
	[XmlRoot("format")]
	public class Format
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("fileFormat")]
		public string FileFormat { get; set; }

		[XmlElement("bitRate")]
		public string BitRate { get; set; }

		[XmlElement("drmFree")]
		public bool DrmFree { get; set; }
	}
}