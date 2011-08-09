using System;

namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class BasketExtensions
	{
		public static IFluentApi<Api.Schema.Basket.Basket> Create(this IFluentApi<Api.Schema.Basket.Basket> api)
		{
			api.WithEndpoint("basket/create");
			return api;
		}

		public static IFluentApi<Api.Schema.Basket.Basket> AddItem(this IFluentApi<Api.Schema.Basket.Basket> api, Guid basketId, int releaseId)
		{
			api.WithEndpoint("basket/additem");
			api.WithParameter("basketId", basketId.ToString());
			api.WithParameter("releaseId", releaseId.ToString());
			return api;
		}

		public static IFluentApi<Api.Schema.Basket.Basket> AddItem(this IFluentApi<Api.Schema.Basket.Basket> api, Guid basketId, int releaseId, int trackId)
		{
			api.WithEndpoint("basket/additem");
			api.WithParameter("basketId", basketId.ToString());
			api.WithParameter("releaseId", releaseId.ToString());
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}

		public static IFluentApi<Api.Schema.Basket.Basket> RemoveItem(this IFluentApi<Api.Schema.Basket.Basket> api, Guid basketId, int itemId)
		{
			api.WithEndpoint("basket/removeitem");
			api.WithParameter("basketId", basketId.ToString());
			api.WithParameter("itemId", itemId.ToString());
			return api;
		}
	}
}