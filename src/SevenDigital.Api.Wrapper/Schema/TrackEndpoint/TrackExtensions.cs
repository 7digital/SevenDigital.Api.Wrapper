namespace SevenDigital.Api.Wrapper.Schema.TrackEndpoint
{
	public static class TrackExtensions
	{
		public static FluentApi<Track> WithTrackId(this FluentApi<Track> api, int trackId)
		{
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}
	}
}