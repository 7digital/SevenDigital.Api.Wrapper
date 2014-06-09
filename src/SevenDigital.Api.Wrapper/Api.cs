using SevenDigital.Api.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper
{
	public static class Api<T> where T : class, new()
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

	public static class Api 
	{
		public static IFluentApi<ArtistSearch> ArtistSearch 
		{
			get { return new FluentApi<ArtistSearch>(); }
		}

		public static IFluentApi<ArtistBrowse> ArtistBrowse 
		{
			get { return new FluentApi<ArtistBrowse>(); }
		}

		public static IFluentApi<ArtistChart> ArtistChart 
		{
			get { return new FluentApi<ArtistChart>(); }
		}
	}
}