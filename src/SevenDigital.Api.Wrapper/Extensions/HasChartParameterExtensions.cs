using System;
using SevenDigital.Api.Schema.Chart;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasChartParameterExtensions
	{
		public static IApiRequest<T> WithPeriod<T>(this IApiRequest<T> api, ChartPeriod period) where T : HasChartParameter
		{
			api.WithParameter("period", period.ToString().ToLower());
			return api;
		}

		public static IApiRequest<T> WithPeriod<T>(this IApiRequest<T> api, string period) where T : HasChartParameter
		{
			api.WithParameter("period", period.ToLower());
			return api;
		}

		public static IApiRequest<T> WithToDate<T>(this IApiRequest<T> api, DateTime toDate) where T : HasChartParameter
		{
			api.WithParameter("toDate", toDate.ToString("yyyyMMdd"));
			return api;
		}
	}
}
