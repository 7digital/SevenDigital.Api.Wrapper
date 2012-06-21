using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Payment
{
	[ApiEndpoint("payment/card/type")]
	[XmlRoot("cardTypes")]
	[Serializable]
	public class PaymentCardTypes
	{
		[XmlElement("cardType")]
		public List<CardType> CardTypes { get; set; }
	}









	[Serializable]
	[XmlRoot("cardType")]
	public class CardType
	{
		[XmlText]
		public string Type { get; set; }

		[XmlAttribute("id")]
		public string Id { get; set; }
	}
}
