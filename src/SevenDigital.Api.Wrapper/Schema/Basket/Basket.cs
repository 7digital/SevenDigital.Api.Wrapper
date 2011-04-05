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

	public static class BasketExtensions
	{
		public static IFluentApi<Basket> Create(this IFluentApi<Basket> api)
		{
			api.WithEndpoint("basket/create");
			return api;
		}

		public static IFluentApi<Basket> AddItem(this IFluentApi<Basket> api, Guid basketId, int releaseId)
		{
			api.WithEndpoint("basket/additem");
			api.WithParameter("basketId", basketId.ToString());
			api.WithParameter("releaseId", releaseId.ToString());
			return api;
		}

		public static IFluentApi<Basket> AddItem(this IFluentApi<Basket> api, Guid basketId, int releaseId, int trackId)
		{
			api.WithEndpoint("basket/additem");
			api.WithParameter("basketId", basketId.ToString());
			api.WithParameter("releaseId", releaseId.ToString());
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}

		public static IFluentApi<Basket> RemoveItem(this IFluentApi<Basket> api, Guid basketId, int itemId)
		{
			api.WithEndpoint("basket/removeitem");
			api.WithParameter("basketId", basketId.ToString());
			api.WithParameter("itemId", itemId.ToString());
			return api;
		}
	}
}
