using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasBasketItemParameterExtensions
	{
		public static IFluentApi<T> BasketItemId<T>(this IFluentApi<T> api, int basketItemId) where T : HasBasketItemParameter
		{
			api.WithIntParameter("itemId", basketItemId);
			return api;
		}
	}
}