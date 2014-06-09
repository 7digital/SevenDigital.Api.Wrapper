using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Responses
{
	public interface IResponseCache
	{
		void Set(RequestData key, object value);
		bool TryGet<T>(RequestData key, out T value);
	}
}
