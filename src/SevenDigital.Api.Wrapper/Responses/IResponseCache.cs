using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Responses
{
	public interface IResponseCache
	{
		void Set(RequestData key, Response value);
		bool TryGet(RequestData key, out Response value);
	}
}
