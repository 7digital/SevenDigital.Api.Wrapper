using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Pricing
{
	
	[XmlRoot("currency")]
	public class Currency
	{
		[XmlAttribute("code")]
		public string Code { get; set; }
	}
}