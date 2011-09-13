using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasKeyParameterExtensions
	{
		public static IFluentApi<T> WithKey<T>(this IFluentApi<T> api, string keyValue) where T : HasKeyParameter
		{
			api.WithParameter("key", keyValue);
			return api;
		}
	}
}
