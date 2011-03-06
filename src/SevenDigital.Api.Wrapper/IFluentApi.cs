namespace SevenDigital.Api.Wrapper
{
	public interface IFluentApi<out T>
	{
		IFluentApi<T> WithParameter(string key, string value);
		T Resolve();
	}
}