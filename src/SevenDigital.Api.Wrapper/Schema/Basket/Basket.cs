using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;
using SevenDigital.Api.Wrapper.Schema.Pricing;

namespace SevenDigital.Api.Wrapper.Schema.Basket
{
	[Serializable]
	[ApiEndpoint("basket")]
	[XmlRoot("basket")]
	public class Basket
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("price")]
		public Price Price { get; set; }

		[XmlElement("basketItems")]
		public BasketItemList BasketItems { get; set; }
	}

	[Serializable]
	[XmlRoot("basketItems")]
	public class BasketItemList
	{
		[XmlElement("basketItem")]
		public List<BasketItem> Items { get; set; }
	}

	[Serializable]
	[XmlRoot("basketItem")]
	public class BasketItem
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("itemName")]
		public string ItemName { get; set; }

		[XmlElement("artistName")]
		public string ArtistName { get; set; }

		[XmlElement("trackId")]
		public string TrackId { get; set; }

		[XmlElement("releaseId")]
		public string ReleaseId { get; set; }

		[XmlElement("price")]
		public Price Price { get; set; }
	}
}
