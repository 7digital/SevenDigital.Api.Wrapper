using SevenDigital.Api.Schema.User.Purchase;

namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class BasePurchaseItemExtensions
	{
		public static IFluentApi<BasePurchaseItem> ForReleaseId(this IFluentApi<BasePurchaseItem> api, int releaseId)
		{
			api.WithParameter("releaseId", releaseId.ToString());
			return api;
		}

		public static IFluentApi<BasePurchaseItem> ForTrackId(this IFluentApi<BasePurchaseItem> api, int trackId)
		{
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}

		public static IFluentApi<BasePurchaseItem> ForPrice(this IFluentApi<BasePurchaseItem> api, decimal price)
		{
			api.WithParameter("price", price.ToString());
			return api;
		}
	}
}