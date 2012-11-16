using System;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasBasketParameterExtensions
	{
		public static IFluentApi<T> UseBasketId<T>(this IFluentApi<T> api, Guid basketId) where T : HasBasketParameter
		{
			api.ClearParameters();
			api.WithParameter("basketId", basketId.ToString());
			return api;
		}
	}
}