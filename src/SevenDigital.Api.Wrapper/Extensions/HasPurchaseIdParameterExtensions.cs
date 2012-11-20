using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPurchaseIdParameterExtensions
	{
		public static IApiRequest<T> WithPurchaseId<T>(this IApiRequest<T> api, int purchaseId) where T : HasPurchaseIdParameter
		{
			api.WithParameter("purchaseId", purchaseId.ToString());
			return api;
		}
	}
}