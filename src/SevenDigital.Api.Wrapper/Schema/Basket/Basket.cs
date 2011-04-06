using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;
using SevenDigital.Api.Wrapper.Schema.Pricing;

namespace SevenDigital.Api.Wrapper.Schema.Basket
{
	[Serializable]
	[ApiEndpoint("basket")]
	[XmlRoot("basket")]
	public class Basket
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("price")]
		public Price Price { get; set; }

		[XmlElement("basketItems")]
		public BasketItemList BasketItems { get; set; }
	}
}
