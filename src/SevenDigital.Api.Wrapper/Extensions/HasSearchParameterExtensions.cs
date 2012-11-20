using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasSearchParameterExtensions
	{
		public static IApiRequest<T> WithQuery<T>(this IApiRequest<T> api, string query) where T : HasSearchParameter
		{
			api.WithParameter("q", query);
			return api;
		}

		public static IApiRequest<T> WithAdvancedQuery<T>(this IApiRequest<T> api, string query) where T : HasSearchParameter
		{
			api.WithParameter("qa", query);
			return api;
		}
	}
}