using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Basket
{
	[XmlRoot("basketItems")]
	public class BasketItemList
	{
		[XmlElement("basketItem")]
		public List<BasketItem> Items { get; set; }
	}
}