using System;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper.Extensions.Get
{
	public static class HasBasketParameterExtensions
	{
		public static IFluentApi<T> WithBasketId<T>(this IFluentApi<T> api, Guid basketId) where T : HasBasketParameter
		{
			return WithBasketId(api, basketId.ToString());
		}

		public static IFluentApi<T> WithBasketId<T>(this IFluentApi<T> api, string basketId) where T : HasBasketParameter
		{
			api.WithEndpoint("basket");
			api.WithParameter("basketId", basketId);
			return api;
		}
	}
}