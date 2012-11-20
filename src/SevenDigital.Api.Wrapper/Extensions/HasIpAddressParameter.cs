using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasIpAddressParameterExtensions
	{
		public static IApiRequest<T> WithIpAddress<T>(this IApiRequest<T> api, string ipAddress) where T : HasIpAddressParameter
		{
			api.WithParameter("ipaddress", ipAddress);
			return api;
		}
	}
}
