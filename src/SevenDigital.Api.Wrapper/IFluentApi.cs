using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Requests.Serializing;
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
		IFluentApi<T> WithMethod(string methodName);
		IFluentApi<T> WithPayload<TPayload>(TPayload payload) where TPayload : class;
		IFluentApi<T> WithPayload<TPayload>(TPayload payload, ITransferContentType transferContentType) where TPayload : class;

		Response Response();
		T Please();
	}
}
