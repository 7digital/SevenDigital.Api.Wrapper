using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Territories
{
	[ApiEndpoint("country/georestrictions/checkout")]
	[XmlRoot("GeoIpLookup")]
	public class CheckoutAvailability : HasIpAddressParameter
	{
		[XmlElement("countryId")]
		public string CountryId { get; set; }

		[XmlElement("allowCheckout")]
		public bool AllowCheckout { get; set; }
	}
}