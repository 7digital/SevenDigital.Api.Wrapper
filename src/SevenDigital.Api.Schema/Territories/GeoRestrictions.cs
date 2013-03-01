using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Territories
{
	[ApiEndpoint("country/georestrictions/checkout")]
	[XmlRoot("geoRestrictions")]
	public class GeoRestrictions : HasIpAddressParameter
	{
		[XmlElement("allowCheckout")]
		public bool AllowCheckout { get; set; }

		[XmlElement("countryCode")]
		public string CountryCode { get; set; }
	}
}
