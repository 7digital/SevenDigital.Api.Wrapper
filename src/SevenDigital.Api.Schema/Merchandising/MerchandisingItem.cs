using System.Xml.Serialization;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Schema.Merchandising
{
	[XmlRoot("merchandisingItem")]
	public class MerchandisingItem
	{
		[XmlElement("type")]
		public string Type{ get; set; }

		[XmlElement("release")]
		public Release Release{ get; set; }
	}
}
