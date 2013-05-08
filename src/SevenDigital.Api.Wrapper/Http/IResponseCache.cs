namespace SevenDigital.Api.Wrapper.Http
{
	public interface IResponseCache
	{
		void Set(RequestData key, Response value);
		bool TryGet(RequestData key, out Response value);
	}
}
