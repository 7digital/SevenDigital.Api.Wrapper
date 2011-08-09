using System;
using SevenDigital.Api.Schema.ParameterDefinitions.Post;

namespace SevenDigital.Api.Wrapper.Extensions.Post
{
	public static class HasBasketParametersExtensions
	{
		public static IFluentApi<T> Create<T>(this IFluentApi<T> api) where T : HasBasketParameters
		{
			api.WithEndpoint("basket/create");
			return api;
		}

		public static IFluentApi<T> AddItem<T>(this IFluentApi<T> api, Guid basketId, int releaseId) where T : HasBasketParameters
		{
			api.WithEndpoint("basket/additem");
			api.WithParameter("basketId", basketId.ToString());
			api.WithParameter("releaseId", releaseId.ToString());
			return api;
		}

		public static IFluentApi<T> AddItem<T>(this IFluentApi<T> api, Guid basketId, int releaseId, int trackId) where T : HasBasketParameters
		{
			api.WithEndpoint("basket/additem");
			api.WithParameter("basketId", basketId.ToString());
			api.WithParameter("releaseId", releaseId.ToString());
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}

		public static IFluentApi<T> RemoveItem<T>(this IFluentApi<T> api, Guid basketId, int itemId) where T : HasBasketParameters
		{
			api.WithEndpoint("basket/removeitem");
			api.WithParameter("basketId", basketId.ToString());
			api.WithParameter("itemId", itemId.ToString());
			return api;
		}
	}
}