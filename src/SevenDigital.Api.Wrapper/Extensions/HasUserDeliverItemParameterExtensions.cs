using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasUserDeliverItemParameterExtensions
	{
		public static IApiRequest<T> WithEmailAddress<T>(this IApiRequest<T> api, string emailAddress) where T : HasUserDeliverItemParameter
		{
			api.WithParameter("emailAddress", emailAddress);
			return api;
		}

		public static IApiRequest<T> WithTransactionId<T>(this IApiRequest<T> api, string transactionId) where T : HasUserDeliverItemParameter
		{
			api.WithParameter("transactionId", transactionId);
			return api;
		}

		public static IApiRequest<T> WithRetailPrice<T>(this IApiRequest<T> api, decimal retailPrice) where T : HasUserDeliverItemParameter
		{
			api.WithParameter("retailPrice", retailPrice.ToString());
			return api;
		}
	}
}