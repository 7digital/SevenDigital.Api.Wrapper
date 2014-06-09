using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Responses
{
	class NullResponseCache : IResponseCache
	{
		public void Set(RequestData key, object value)
		{
			// don't store it
		}

		public bool TryGet<T>(RequestData key, out T value)
		{
			// nope, I don't have it
			value = default(T);
			return false;
		}
	}
}