namespace SevenDigital.Api.Wrapper
{
	public interface IApi
	{
		IFluentApi<T> Create<T>() where T : class, new();
	}
	
	public class ApiFactory: IApi
	{
		public IFluentApi<T> Create<T>() where T : class, new()
		{
			return new FluentApi<T>();
		}
	}
}