using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPurchaseIdParameterExtensions
	{
		public static IFluentApi<T> WithPurchaseId<T>(this IFluentApi<T> api, int purchaseId) where T : HasPurchaseIdParameter
		{
			api.WithIntParameter("purchaseId", purchaseId);
			return api;
		}
	}
}