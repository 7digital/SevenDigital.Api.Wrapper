namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class AffiliatePartnerExtensions
	{
		public static IFluentApi<T> ForAffiliatePartner<T>(this IFluentApi<T> api, int partnerId)
		{
			api.WithParameter("affiliatePartner", partnerId.ToString());
			return api;
		}
	}
}
