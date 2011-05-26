using System;
using SevenDigital.Api.Schema.Chart;
using SevenDigital.Api.Schema.TrackEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.TrackEndpoint
{
	public static class TrackExtensions
	{
		public static IFluentApi<Track> WithTrackId(this IFluentApi<Track> api, int trackId)
		{
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}
	}

	public static class TrackChartExtensions {
		public static IFluentApi<TrackChart> WithPeriod(this IFluentApi<TrackChart> api, ChartPeriod period) {
			api.WithParameter("period", period.ToString().ToLower());
			return api;
		}

		public static IFluentApi<TrackChart> WithPeriod(this IFluentApi<TrackChart> api, string period) {
			api.WithParameter("period", period.ToLower());
			return api;
		}

		public static IFluentApi<TrackChart> WithToDate(this IFluentApi<TrackChart> api, DateTime toDate) {
			api.WithParameter("toDate", toDate.ToString("yyyyMMdd"));
			return api;
		}
	}
}