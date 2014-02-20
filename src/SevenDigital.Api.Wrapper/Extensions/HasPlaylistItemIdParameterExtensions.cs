using SevenDigital.Api.Schema.Playlists;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPlaylistItemIdParameterExtensions
	{
		public static IFluentApi<T> ForPlaylistItemId<T>(this IFluentApi<T> api, string playlistItemId) where T : HasPlaylistIdParameter
		{
			api.WithParameter("playlistItemId", playlistItemId);
			return api;
		}
	}
}