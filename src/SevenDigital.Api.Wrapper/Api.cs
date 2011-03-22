namespace SevenDigital.Api.Wrapper
{
	public static class Api<T> where T : class
	{
		public static IFluentApi<T> Get
		{
			get { 
				var api = new FluentApi<T>();
				return api.WithMethod("GET");
			}
		}

		public static IFluentApi<T> Post
		{
			get
			{
				var api = new FluentApi<T>();
				return api.WithMethod("POST");
			}
		}
	}
}