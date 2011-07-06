using SevenDigital.Api.Schema.FilterDefinitions;

namespace SevenDigital.Api.Wrapper.Schema
{
	public static class RequiresKeyParameterExtensions
	{
		public static IFluentApi<T> WithKey<T>(this IFluentApi<T> api, string keyValue) where T : RequiresKeyParameter
		{
			api.WithParameter("key", keyValue);
			return api;
		}
	}
}
