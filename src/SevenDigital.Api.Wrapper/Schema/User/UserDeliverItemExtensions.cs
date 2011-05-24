using SevenDigital.Api.Schema.User;

namespace SevenDigital.Api.Wrapper.Schema.User
{
	public static class UserDeliverItemExtensions
	{
		public static IFluentApi<UserDeliverItem> ForReleaseId(this IFluentApi<UserDeliverItem> api, int releaseId)
		{
			api.WithParameter("releaseId", releaseId.ToString());
			return api;
		}

		public static IFluentApi<UserDeliverItem> ForTrackId(this IFluentApi<UserDeliverItem> api, int trackId)
		{
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}

		public static IFluentApi<UserDeliverItem> WithEmailAddress(this IFluentApi<UserDeliverItem> api, string emailAddress)
		{
			api.WithParameter("emailAddress", emailAddress);
			return api;
		}

		public static IFluentApi<UserDeliverItem> WithTransactionId(this IFluentApi<UserDeliverItem> api, string transactionId)
		{
			api.WithParameter("transactionId", transactionId);
			return api;
		}

		public static IFluentApi<UserDeliverItem> WithRetailPrice(this IFluentApi<UserDeliverItem> api, decimal retailPrice)
		{
			api.WithParameter("retailPrice", retailPrice.ToString());
			return api;
		}
	}
}