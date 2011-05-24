using System;
using SevenDigital.Api.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.ArtistEndpoint
{
	public static class ArtistExtensions
	{
		public static IFluentApi<IIsArtist> WithArtistId(this IFluentApi<IIsArtist> api, int artistId)
		{
			api.WithParameter("artistId", artistId.ToString());
			return api;
		}
	}

	public static class ArtistBrowseExtensions
	{
		public static IFluentApi<ArtistBrowse> WithLetter(this IFluentApi<ArtistBrowse> api, string letter)
		{
			api.WithParameter("letter", letter);
			return api;
		}
	}

	public static class ArtistChartExtensions
	{
		public static IFluentApi<ArtistChart> WithPeriod(this IFluentApi<ArtistChart> api, ApiPeriod period)
		{
			api.WithParameter("period", period.ToString().ToLower());
			return api;
		}

		public static IFluentApi<ArtistChart> WithToDate(this IFluentApi<ArtistChart> api, DateTime toDate)
		{
			api.WithParameter("toDate", toDate.ToString("yyyyMMdd"));
			return api;
		}
	}

	public static class ArtistSearchExtensions
	{
		public static IFluentApi<ArtistSearch> WithQuery(this IFluentApi<ArtistSearch> api, string query)
		{
			api.WithParameter("q", query);
			return api;
		}

		public static IFluentApi<ArtistSearch> WithAdvancedQuery(this IFluentApi<ArtistSearch> api, string query)
		{
			api.WithParameter("qa", query);
			return api;
		}
	}
}