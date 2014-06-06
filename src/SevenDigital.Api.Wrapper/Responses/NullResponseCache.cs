using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Responses
{
	class NullResponseCache : IResponseCache
	{
		public void Set(RequestData key, object value)
		{
			// don't store it
		}

		public bool TryGet(RequestData key, out object value)
		{
			// nope, I don't have it
			value = null;
			return false;
		}
	}
}