using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasLetterParameterExtensions
	{
		public static IFluentApi<T> WithLetter<T>(this IFluentApi<T> api, string letter) where T : HasLetterParameter
		{
			api.WithParameter("letter", letter);
			return api;
		}
	}
}