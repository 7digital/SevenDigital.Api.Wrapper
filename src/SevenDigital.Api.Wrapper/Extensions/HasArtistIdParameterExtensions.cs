using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasArtistIdParameterExtensions
	{
		public static IApiRequest<T> WithArtistId<T>(this IApiRequest<T> api, int artistId) where T : HasArtistIdParameter
		{
			api.WithParameter("artistId", artistId.ToString());
			return api;
		}
	}
}