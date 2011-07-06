using SevenDigital.Api.Schema.FilterDefinitions;

namespace SevenDigital.Api.Wrapper.Schema
{
	public static class RequiresKeyParameterExtensions
	{
		public static IFluentApi<T> WithName<T>(this IFluentApi<T> api, string listName) where T : RequiresKeyParameter
		{
			api.WithParameter("key", listName);
			return api;
		}
	}
}
