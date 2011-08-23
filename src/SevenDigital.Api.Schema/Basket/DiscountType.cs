using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Basket
{
	[XmlRoot("type")]
	public enum DiscountType
	{
		Undefined,
		Voucher
	}
}