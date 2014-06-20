namespace SevenDigital.Api.Wrapper
{
	public static class StaticApiFactory
	{
		public static IApi Factory { get; set; }
	}

	public static class Api<T> where T : class, new()
	{
		public static IFluentApi<T> Create
		{
			get
			{
				if (StaticApiFactory.Factory == null)
				{
					StaticApiFactory.Factory = new ApiFactory();
				}
				return StaticApiFactory.Factory.Create<T>();
			}
		}
	}
}
