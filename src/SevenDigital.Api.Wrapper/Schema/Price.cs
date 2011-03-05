using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Schema
{
	[Serializable]
	[XmlRoot("price")]
	public class Price
	{
		[XmlElement("currency")]
		public Currency Currency { get; set; }

		[XmlElement("value")]
		public int Value { get; set; }

		[XmlElement("formattedPrice")]
		public int FormattedPrice { get; set; }

		[XmlElement("rrp")]
		public decimal Rrp { get; set; }

		[XmlElement("formattedRrp")]
		public string FormattedRrp { get; set; }

		[XmlElement("isOnSale")]
		public bool IsOnSale { get; set; }
	}
}