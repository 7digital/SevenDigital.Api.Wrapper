using SevenDigital.Api.Schema.LockerEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.LockerEndpoint
{
	public static class LockerExtensions
	{
		public static IFluentApi<Locker> ForReleaseId(this IFluentApi<Locker> api, int releaseId)
		{
			api.WithParameter("releaseId", releaseId.ToString());
			return api;
		}

		public static IFluentApi<Locker> ForTrackId(this IFluentApi<Locker> api, int trackId)
		{
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}
	}
}