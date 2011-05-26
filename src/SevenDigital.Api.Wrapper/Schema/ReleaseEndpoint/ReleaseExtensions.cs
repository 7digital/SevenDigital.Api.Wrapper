using System;
using SevenDigital.Api.Schema.Chart;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint
{
	public static class ReleaseChartExtensions {
		public static IFluentApi<ReleaseChart> WithPeriod(this IFluentApi<ReleaseChart> api, ChartPeriod period) {
			api.WithParameter("period", period.ToString().ToLower());
			return api;
		}

		public static IFluentApi<ReleaseChart> WithPeriod(this IFluentApi<ReleaseChart> api, string period) {
			api.WithParameter("period", period.ToLower());
			return api;
		}

		public static IFluentApi<ReleaseChart> WithToDate(this IFluentApi<ReleaseChart> api, DateTime toDate) {
			api.WithParameter("toDate", toDate.ToString("yyyyMMdd"));
			return api;
		}
	}
}