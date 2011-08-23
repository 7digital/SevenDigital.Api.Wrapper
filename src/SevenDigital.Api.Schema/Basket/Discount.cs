using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Basket
{
	[XmlRoot("discount")]
	public class Discount
	{
		[XmlElement("type")]
		public DiscountType Type { get; set; }
	}
}