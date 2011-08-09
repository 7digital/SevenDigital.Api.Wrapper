using System;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper.Extensions.Get
{
	public static class HasBasketIdParameterExtensions
	{
		public static IFluentApi<T> WithBasketId<T>(this IFluentApi<T> api, Guid basketId) where T : HasBasketIdParameter
		{
			api.WithParameter("basketId", basketId.ToString());
			return api;
		}
	}
}