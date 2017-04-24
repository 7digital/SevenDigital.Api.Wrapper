using System.Linq;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasUsageTypesParameterExtensions
	{
		public static IFluentApi<T> ForUsageTypes<T>(this IFluentApi<T> api, params UsageType[] usageTypes) where T : HasUsageTypesParameter
		{
			api.WithParameter("usageTypes", string.Join(",", usageTypes.Select(o => o.ToString().ToLowerInvariant())));
			return api;
		}
	}
}
