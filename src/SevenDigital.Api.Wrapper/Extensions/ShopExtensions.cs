using System.Globalization;

namespace SevenDigital.Api.Wrapper
{
	public static class ShopExtensions
	{
		public static IFluentApi<T> ForShop<T>(this IFluentApi<T> api, int shopId)
		{
			return api.WithParameter("shopId", shopId.ToString(CultureInfo.InvariantCulture));
		}
	}
}
