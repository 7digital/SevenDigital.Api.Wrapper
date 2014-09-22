using System.Net.Http;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper
{
	public static class ApiExtensions
	{
		public static IFluentApi<T> UsingBaseUri<T>(this IFluentApi<T> api, string baseUri)
		{
			api.UsingBaseUri(new BaseUriFromString(baseUri));
			return api;
		}

		public static IFluentApi<T> WithPost<T>(this IFluentApi<T> fluentApi)
		{
			return fluentApi.WithMethod(HttpMethod.Post);
		}

		public static IFluentApi<T> AcceptJson<T>(this IFluentApi<T> fluentApi)
		{
			return fluentApi.WithAccept("application/json");
		}
	}
}
