using System;
using System.Linq;
using SevenDigital.Api.Schema.Playlists;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPlaylistIdParameterExtensions
	{
		public static IFluentApi<T> ForPlaylistId<T>(this IFluentApi<T> api, string playlistId) where T : HasPlaylistIdParameter
		{
			api.WithParameter("playlistId", playlistId);
			return api;
		}

		public static IFluentApi<T> ForPlaylistUrl<T>(this IFluentApi<T> api, Uri playlistUrl) where T : HasPlaylistIdParameter
		{
			var playlistId = playlistUrl.Segments.Last();
			api.WithParameter("playlistId", playlistId);
			return api;
		}
	}
}