using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Territories
{
	[ApiEndpoint("country/resolve")]
	[XmlRoot("GeoIpLookup")]
	public class GeoIpLookup : HasIpAddressParameter
	{
		[XmlElement("ipaddress")]
		public string IpAddress { get; set; }

		[XmlElement("countryCode")]
		public string CountryCode { get; set; }
	}
}