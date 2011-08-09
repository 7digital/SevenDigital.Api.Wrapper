using System;
using SevenDigital.Api.Schema.User.Purchase;

namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class UserPurchaseBasketExtensions
	{
		public static IFluentApi<BasePurchaseItem> WithBasketId(this IFluentApi<BasePurchaseItem> api, Guid basketId)
		{
			api.WithParameter("basketId", basketId.ToString());
			return api;
		}
	}
}