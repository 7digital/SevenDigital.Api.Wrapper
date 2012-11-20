using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper 
{
	public static class HasTrackIdParameterExtensions 
	{
		public static IApiRequest<T> ForTrackId<T>(this IApiRequest<T> api, int trackId) where T : HasTrackIdParameter 
		{
			api.WithParameter("trackId", trackId.ToString());
			return api;
		}
	}
}