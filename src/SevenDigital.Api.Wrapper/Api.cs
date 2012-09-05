using SevenDigital.Api.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper
{
	public static class Api<T> where T : class
	{
		public static IFluentApi<T> Create
		{
			get { return new FluentApi<T>(); }
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