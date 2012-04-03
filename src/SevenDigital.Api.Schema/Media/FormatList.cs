using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Media
{
	
	[XmlRoot("formatList")]
	[Serializable]
	public class FormatList
	{
		[XmlAttribute("availableDrmFree")]
		public bool AvailableDrmFree { get; set; }

		[XmlElement("format")]
		public List<Format> Formats { get; set; }
	}
}