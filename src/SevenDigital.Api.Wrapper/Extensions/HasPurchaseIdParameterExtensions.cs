using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPurchaseIdParameterExtensions
	{
		public static IFluentApi<T> ForPurchaseId<T>(this IFluentApi<T> api, int basketId) where T : HasPurchaseIdParameter
		{
			api.WithParameter("basketId", basketId.ToString());
			return api;
		}
	}
}