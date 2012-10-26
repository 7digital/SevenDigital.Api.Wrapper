using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Pricing
{
	[XmlRoot("price")]
	[Serializable]
	public class Price
	{
		[XmlElement("currency")]
		public Currency Currency { get; set; }

		[XmlElement("value")]
		public string Value { get; set; }

		[XmlElement("formattedPrice")]
		public string FormattedPrice { get; set; }

		[XmlElement("rrp")]
		public string Rrp { get; set; }

		[XmlElement("formattedRrp")]
		public string FormattedRrp { get; set; }

		[XmlElement("onSale")]
		public bool IsOnSale { get; set; }

		public PriceStatus Status
		{
			get
			{
				if (PriceIsZero())
				{
					return PriceStatus.Free;
				}

				if(string.IsNullOrEmpty(Value) && FormattedPrice == "N/A")
				{
					return PriceStatus.UnAvailable;
				}

				return PriceStatus.Available;
			}
		}

		private bool PriceIsZero()
		{
			decimal value;
			if (decimal.TryParse(Value, out value))
			{
				return value == 0;
			}
			
			return false;
		}
	}
}