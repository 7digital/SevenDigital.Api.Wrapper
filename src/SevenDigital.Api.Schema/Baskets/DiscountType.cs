using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Baskets
{
	[XmlRoot("type")]
	public enum DiscountType
	{
		Undefined,
		Voucher
	}
}