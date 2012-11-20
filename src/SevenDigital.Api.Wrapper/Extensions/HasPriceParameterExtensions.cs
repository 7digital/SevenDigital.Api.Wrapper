using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPriceParameterExtensions
	{
		public static IApiRequest<T> ForPrice<T>(this IApiRequest<T> api, decimal price) where T : HasPriceParameter
		{
			api.WithParameter("price", price.ToString());
			return api;
		}
	}
}