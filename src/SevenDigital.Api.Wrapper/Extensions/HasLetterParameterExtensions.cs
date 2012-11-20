using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasLetterParameterExtensions
	{
		public static IApiRequest<T> WithLetter<T>(this IApiRequest<T> api, string letter) where T : HasLetterParameter
		{
			api.WithParameter("letter", letter);
			return api;
		}
	}
}