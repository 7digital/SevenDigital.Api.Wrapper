using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.Playlists.Response.Endpoints
{
	[OAuthSigned]
	[ApiEndpoint("playlists/{playlistId}/tracks/{playlistTrackId}")]
	public class PlaylistTrack : HasPlaylistIdParameter, HasPlaylistTrackIdParameter
	{}
}