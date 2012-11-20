using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasReleaseIdParameterExtensions
	{
		public static IApiRequest<T> ForReleaseId<T>(this IApiRequest<T> api, int releaseId) where T : HasReleaseIdParameter
		{
			api.WithParameter("releaseId", releaseId.ToString());
			return api;
		}
	}
}