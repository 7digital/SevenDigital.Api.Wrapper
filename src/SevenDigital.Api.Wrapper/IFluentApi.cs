using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper
{
	public interface IFluentApi<T> : IApiEndpoint
	{
		IFluentApi<T> WithParameter(string key, string value);
		IFluentApi<T> ClearParameters();
		IFluentApi<T> ForUser(string token, string secret);
		IFluentApi<T> UsingClient(IHttpClient httpClient);
		IFluentApi<T> UsingCache(IResponseCache responseCache);

		T Please();
	}
}
