namespace SevenDigital.Api.Wrapper
{
	public static class ApiForward
	{
		public static IApi ApiFactory { get; set; }
	}

	public static class Api<T> where T : class, new()
	{
		public static IFluentApi<T> Create
		{
			get
			{
				if (ApiForward.ApiFactory == null)
				{
					ApiForward.ApiFactory = new ApiFactory();
				}
				return ApiForward.ApiFactory.Create<T>();
			}
		}
	}
}
