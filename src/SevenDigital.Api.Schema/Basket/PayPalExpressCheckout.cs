using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Basket
{
	[ApiEndpoint("basket/paypalurl")]
	[XmlRoot("expressCheckout")]
	public class PayPalExpressCheckout : HasBasketParameter
	{
		[XmlElement("url")]
		public string Url { get; set; }
	}
}