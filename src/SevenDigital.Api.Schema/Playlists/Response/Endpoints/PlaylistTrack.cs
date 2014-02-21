using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.Playlists.Response.Endpoints
{
	[OAuthSigned]
	[ApiEndpoint("playlists/{playlistId}/tracks/{playlistItemId}")]
	public class PlaylistItem : HasPlaylistIdParameter, HasPlaylistItemIdParameter
	{}
}