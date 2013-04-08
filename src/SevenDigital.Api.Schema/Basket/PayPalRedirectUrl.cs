using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Basket
{
	[ApiEndpoint("basket/paypalurl")]
	[XmlRoot("paypalurl")]
	public class PayPalRedirectUrl : HasBasketParameter
	{
	}
}