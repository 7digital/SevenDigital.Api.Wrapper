using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasKeyParameterExtensions
	{
		public static IApiRequest<T> WithKey<T>(this IApiRequest<T> api, string keyValue) where T : HasKeyParameter
		{
			api.WithParameter("key", keyValue);
			return api;
		}
	}
}
