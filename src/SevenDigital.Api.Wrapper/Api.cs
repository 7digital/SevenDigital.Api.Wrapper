using SevenDigital.Api.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper
{
	public static class Api<T> where T : class
	{
		public static IFluentApi<T> Create
		{
			get { 
				var api = new FluentApi<T>();
				return api;
			}
		}
	}

	public static class Api {
		public static IFluentApi<ArtistSearch> ArtistSearch {
			get { return new FluentApi<ArtistSearch>(); }
		}

		public static IFluentApi<ArtistBrowse> ArtistBrowse {
			get { return new FluentApi<ArtistBrowse>(); }
		}

		public static IFluentApi<ArtistChart> ArtistChart {
			get { return new FluentApi<ArtistChart>(); }
		}
	}
}