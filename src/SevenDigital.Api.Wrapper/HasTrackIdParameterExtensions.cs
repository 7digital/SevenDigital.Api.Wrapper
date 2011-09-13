using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper {
	public static class HasTrackIdParameterExtensions {

		public static IFluentApi<T> ForTrackId<T>(this IFluentApi<T> api, int trackId) where T : HasTrackIdParameter {
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}
	}
}