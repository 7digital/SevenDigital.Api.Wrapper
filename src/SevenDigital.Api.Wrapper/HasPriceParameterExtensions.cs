using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPriceParameterExtensions{

		public static IFluentApi<T> ForPrice<T>(this IFluentApi<T> api, decimal price) where T : HasPriceParameter
		{
			api.WithParameter("price", price.ToString());
			return api;
		}
	}
}