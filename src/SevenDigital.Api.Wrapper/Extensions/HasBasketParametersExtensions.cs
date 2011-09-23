using System;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasBasketParametersExtensions
	{
		public static IFluentApi<T> Create<T>(this IFluentApi<T> api) where T : HasBasketParameter
		{
			api.ClearParameters();
			api.WithEndpoint("basket/create");
			return api;
		}

		public static IFluentApi<T> GetBasket<T>(this IFluentApi<T> api, Guid basketId) where T : HasBasketParameter
		{
			return api.GetBasket(basketId.ToString());
		}

		public static IFluentApi<T> GetBasket<T>(this IFluentApi<T> api, string basketId) where T : HasBasketParameter
		{
			api.ClearParameters();

			api.WithEndpoint("basket");
			api.WithParameter("basketId", basketId);

			return api;
		}

		public static IFluentApi<T> AddItem<T>(this IFluentApi<T> api, Guid basketId, int releaseId) where T : HasBasketParameter
		{
			return api.AddItem(basketId.ToString(), releaseId);
		}

		public static IFluentApi<T> AddItem<T>(this IFluentApi<T> api, string basketId, int releaseId) where T : HasBasketParameter
		{
			api.ClearParameters();
			api.WithEndpoint("basket/additem");
	
			api.WithParameter("basketId", basketId);
			api.WithParameter("releaseId", releaseId.ToString());
			

			return api;
		}

		public static IFluentApi<T> AddItem<T>(this IFluentApi<T> api, Guid basketId, int releaseId, int itemId) where T : HasBasketParameter
		{
			return AddItem(api, basketId.ToString(), releaseId, itemId);
		}

		public static IFluentApi<T> AddItem<T>(this IFluentApi<T> api, string basketId, int releaseId, int itemId) where T : HasBasketParameter
		{
			api.WithEndpoint("basket/additem");
			api.WithParameter("basketId", basketId);
			api.WithParameter("releaseId", releaseId.ToString());
			api.WithParameter("trackId", itemId.ToString());
			return api;
		}

		public static IFluentApi<T> RemoveItem<T>(this IFluentApi<T> api, Guid basketId, int itemId) where T : HasBasketParameter
		{
			return RemoveItem(api, basketId.ToString(), itemId);
		}
		public static IFluentApi<T> RemoveItem<T>(this IFluentApi<T> api, string basketId, int itemId) where T : HasBasketParameter
		{
			api.WithEndpoint("basket/removeitem");
			api.WithParameter("basketId", basketId);
			api.WithParameter("itemId", itemId.ToString());
			return api;
		}
	}
}