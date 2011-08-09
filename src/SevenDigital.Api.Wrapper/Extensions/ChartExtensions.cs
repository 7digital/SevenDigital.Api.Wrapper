using System;
using SevenDigital.Api.Schema.Chart;

namespace SevenDigital.Api.Wrapper.Schema
{
	public static class ChartExtensions
	{
		public static IFluentApi<T> WithPeriod<T>(this IFluentApi<T> api, ChartPeriod period) where T : IIsChart
		{
			api.WithParameter("period", period.ToString().ToLower());
			return api;
		}

		public static IFluentApi<T> WithPeriod<T>(this IFluentApi<T> api, string period) where T : IIsChart
		{
			api.WithParameter("period", period.ToLower());
			return api;
		}

		public static IFluentApi<T> WithToDate<T>(this IFluentApi<T> api, DateTime toDate) where T : IIsChart
		{
			api.WithParameter("toDate", toDate.ToString("yyyyMMdd"));
			return api;
		}
	}
}
