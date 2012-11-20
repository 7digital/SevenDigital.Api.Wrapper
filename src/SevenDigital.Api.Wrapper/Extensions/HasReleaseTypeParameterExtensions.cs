using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class HasReleaseTypeParameterExtensions
	{
		public static IApiRequest<T> ForReleaseType<T>(this IApiRequest<T> api, ReleaseType releaseType) where T : HasReleaseTypeParameter
		{
			return api.ForReleaseType(releaseType.ToString());
		}

		public static IApiRequest<T> ForReleaseType<T>(this IApiRequest<T> api, string releaseType) where T : HasReleaseTypeParameter
		{
			api.WithParameter("type", releaseType.ToString());
			return api;
		}
	}
}