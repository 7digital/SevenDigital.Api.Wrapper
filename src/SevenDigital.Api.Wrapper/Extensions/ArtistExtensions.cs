using SevenDigital.Api.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class ArtistExtensions
	{
		public static IFluentApi<T> WithArtistId<T>(this IFluentApi<T> api, int artistId) where T : IIsArtist
		{
			api.WithParameter("artistId", artistId.ToString());
			return api;
		}
	}

	public static class ArtistBrowseExtensions
	{
		public static IFluentApi<T> WithLetter<T>(this IFluentApi<T> api, string letter) where T : IIsBrowseable
		{
			api.WithParameter("letter", letter);
			return api;
		}
	}

	public static class ArtistSearchExtensions
	{
		public static IFluentApi<T> WithQuery<T>(this IFluentApi<T> api, string query) where T : IIsSearchable
		{
			api.WithParameter("q", query);
			return api;
		}

		public static IFluentApi<T> WithAdvancedQuery<T>(this IFluentApi<T> api, string query) where T : IIsSearchable
		{
			api.WithParameter("qa", query);
			return api;
		}
	}
}