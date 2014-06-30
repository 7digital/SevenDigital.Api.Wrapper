using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Baskets
{
	[ApiEndpoint("basket/paypalurl")]
	[XmlRoot("expressCheckout")]
	public class PayPalExpressCheckout : HasBasketParameter
	{
		[XmlElement("url")]
		public string Url { get; set; }
	}

	[OAuthSigned]
	[HttpPost]
	[ApiEndpoint("basket/completepaypalpurchase")]
	[XmlRoot("purchase")]
	public class CompletePayPalExpressCheckout : HasBasketParameter
	{
		[XmlAttribute("id")]
		public string PurchaseId { get; set; }
	}
}