using SevenDigital.Api.Schema.TrackEndpoint;

namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class TrackExtensions
	{
		public static IFluentApi<Track> WithTrackId(this IFluentApi<Track> api, int trackId)
		{
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}
	}
}