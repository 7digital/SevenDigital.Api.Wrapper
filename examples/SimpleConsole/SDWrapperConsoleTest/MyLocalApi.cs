using SevenDigital.Api.Wrapper;

namespace SDWrapperConsoleTest
{
	/// <summary>
	/// Of the advantages of this approach, 
	/// see "Instancing" section of https://github.com/7digital/SevenDigital.Api.Wrapper/blob/master/ReleaseNotes40.md
	/// </summary>
	public class MyLocalApi : IApi
	{
		private readonly ApiFactory _apiFactory;

		public MyLocalApi()
		{
			var apiUri = new ApiUri();
			var apiCredentials = new ApiCredentials();
			_apiFactory = new ApiFactory(apiUri, apiCredentials);
		}

		public IFluentApi<T> Create<T>() where T : class, new()
		{
			var request = _apiFactory.Create<T>();

			// custom config applied to all requests at this point
			// e.g. request.UsingCache(myCache); or request.WithParameter("foo", "bar");

			return request;
		}
	}
}
