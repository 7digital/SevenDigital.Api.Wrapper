using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Responses
{
	public interface IResponseCache
	{
		void Set(Response response, object value);
		bool TryGet<T>(RequestData request, out T value);
	}
}
