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

	public class Api<T> where T : class, new()
	{
		public static IFluentApi<T> Create
		{
			get { return new FluentApi<T>(); }
		}

		public static IFluentApi<T> CreateWithCreds(IOAuthCredentials oAuthCredentials, IApiUri apiUri)
		{
			return new FluentApi<T>(oAuthCredentials, apiUri);
		}
	}
}