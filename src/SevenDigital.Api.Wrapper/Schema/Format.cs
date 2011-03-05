using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Schema
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
		public int BitRate { get; set; }
		
		[XmlElement("drmFree")]
		public bool DrmFree { get; set; }
	}
}