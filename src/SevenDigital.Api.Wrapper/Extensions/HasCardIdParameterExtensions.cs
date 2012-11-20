using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasCardIdParameterExtensions
	{
		public static IApiRequest<T> WithCard<T>(this IApiRequest<T> api, int cardId) where T : HasCardIdParameter
		{
			api.WithParameter("cardId", cardId.ToString());
			return api;
		}
	}
}