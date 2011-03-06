using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Schema.Pricing
{
	[Serializable]
	[XmlRoot("currency")]
	public class Currency
	{
		[XmlAttribute("code")]
		public string Code { get; set; }
	}
}