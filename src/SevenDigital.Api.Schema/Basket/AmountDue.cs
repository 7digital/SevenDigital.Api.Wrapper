using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Basket
{
	[XmlRoot("amountDue")]
	public class AmountDue
	{
		[XmlElement("amount")]
		public string Amount { get; set; }

		[XmlElement("formattedAmount")]
		public string FormattedAmount { get; set; }
	}
}