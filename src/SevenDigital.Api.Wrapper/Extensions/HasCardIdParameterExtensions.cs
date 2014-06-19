using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasCardIdParameterExtensions
	{
		public static IFluentApi<T> WithCard<T>(this IFluentApi<T> api, int cardId) where T : HasCardIdParameter
		{
			api.WithParameter("cardId", cardId);
			return api;
		}
	}
}