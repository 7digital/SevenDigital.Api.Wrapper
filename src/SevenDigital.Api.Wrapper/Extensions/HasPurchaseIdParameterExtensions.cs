using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPurchaseIdParameter
	{
		public static IFluentApi<T> WithPurchaseId<T>(this IFluentApi<T> api, string basketId) where T : HasBasketParameter
		{
			api.WithParameter("basketId", basketId);
			return api;
		}
	}
}