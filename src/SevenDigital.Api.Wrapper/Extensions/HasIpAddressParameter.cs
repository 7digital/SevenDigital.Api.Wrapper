using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasIpAddressParameterExtensions
	{
		public static IFluentApi<T> WithIpAddress<T>(this IFluentApi<T> api, string ipAddress) where T : HasIpAddressParameter
		{
			api.WithParameter("ipaddress", ipAddress);
			return api;
		}
	}
}
