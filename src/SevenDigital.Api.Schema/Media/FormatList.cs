using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Media
{
	[Serializable]
	[XmlRoot("formatList")]
	public class FormatList
	{
		[XmlAttribute("availableDrmFree")]
		public bool AvailableDrmFree { get; set; }

		[XmlElement("format")]
		public List<Format> Formats { get; set; }
	}
}