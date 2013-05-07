namespace SevenDigital.Api.Wrapper.Http
{
	class NullCache : ICache
	{
		public void Set(string key, string value)
		{
			// do nothing
		}

		public bool TryGet(string key, out string value)
		{
			value = string.Empty;
			return false;
		}
	}
}
