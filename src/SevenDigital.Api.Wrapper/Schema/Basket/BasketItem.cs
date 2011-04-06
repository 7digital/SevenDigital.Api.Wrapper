using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Pricing;

namespace SevenDigital.Api.Wrapper.Schema.Basket
{
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