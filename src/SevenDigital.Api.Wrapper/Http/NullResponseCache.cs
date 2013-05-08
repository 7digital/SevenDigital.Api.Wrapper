namespace SevenDigital.Api.Wrapper.Http
{
	class NullResponseCache : IResponseCache
	{
		public void Set(RequestData key, Response value)
		{
			// don't store it
		}

		public bool TryGet(RequestData key, out Response value)
		{
			// nope, I don't have it
			value = null;
			return false;
		}
	}
}