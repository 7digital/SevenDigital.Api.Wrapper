using SevenDigital.Api.Wrapper.Environment;

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
					var apiUri = EssentialDependencyCheck<IApiUri>.Instance;
					var oAuthConsumerCredentials = EssentialDependencyCheck<IOAuthCredentials>.Instance;
					StaticApiFactory.Factory = new ApiFactory(apiUri, oAuthConsumerCredentials);
				}
				return StaticApiFactory.Factory.Create<T>();
			}
		}
	}
}
