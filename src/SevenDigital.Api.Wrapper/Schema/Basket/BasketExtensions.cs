using System;

namespace SevenDigital.Api.Wrapper.Schema.Basket
{
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