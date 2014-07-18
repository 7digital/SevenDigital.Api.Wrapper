namespace SevenDigital.Api.Wrapper
{
	public static class ApiExtensions
	{
		public static IFluentApi<T> UsingBaseUri<T>(this IFluentApi<T> api, string baseUri)
		{
			api.UsingBaseUri(new StringBaseUriProvider(baseUri));
			return api;
		}
	}
}
