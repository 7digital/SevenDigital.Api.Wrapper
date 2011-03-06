namespace SevenDigital.Api.Wrapper.Repository
{
	public interface IFluentApi<T>
	{
		//IFluentApi<T> WithMethod(string httpMethod);
		IFluentApi<T> WithParameter(string key, string value);
		T Resolve();
	}
}