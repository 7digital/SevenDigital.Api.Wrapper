using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasReleaseIdParameterExtensions
	{
		public static IFluentApi<T> ForReleaseId<T>(this IFluentApi<T> api, int releaseId) where T : HasReleaseIdParameter
		{
			api.WithParameter("releaseId", releaseId.ToString());
			return api;
		}
	}
}