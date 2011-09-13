using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasSearchParameterExtensions
	{
		public static IFluentApi<T> WithQuery<T>(this IFluentApi<T> api, string query) where T : HasSearchParameter
		{
			api.WithParameter("q", query);
			return api;
		}

		public static IFluentApi<T> WithAdvancedQuery<T>(this IFluentApi<T> api, string query) where T : HasSearchParameter
		{
			api.WithParameter("qa", query);
			return api;
		}
	}
}