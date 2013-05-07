namespace SevenDigital.Api.Wrapper.Http
{
	public interface ICache
	{
		void Set(string key, string value);
		bool TryGet(string key, out string value);
	}
}
