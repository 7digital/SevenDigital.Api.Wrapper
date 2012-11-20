using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasBasketItemParameterExtensions
	{
		public static IApiRequest<T> BasketItemId<T>(this IApiRequest<T> api, int basketItemId) where T : HasBasketItemParameter
		{
			api.WithParameter("itemId", basketItemId.ToString());
			return api;
		}
	}
}