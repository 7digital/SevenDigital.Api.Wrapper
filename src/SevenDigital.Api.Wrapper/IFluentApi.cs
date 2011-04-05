namespace SevenDigital.Api.Wrapper
{
	public interface IFluentApi<out T>
	{
		IFluentApi<T> WithParameter(string key, string value);
	    IFluentApi<T> ForUser(string token, string secret);
		IFluentApi<T> WithEndpoint(string endpoint);
		T Please();
	}
}