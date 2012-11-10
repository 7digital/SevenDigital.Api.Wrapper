using System;
using System.Linq;
using System.Threading.Tasks;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.LockerEndpoint;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.ExampleUsage
{
	class Program
	{
		static void Main(string[] args)
		{
			string s = args[0];

			var appSettingsCredentials = new AppSettingsCredentials();
			Console.WriteLine("Using creds: {0} - {1}", appSettingsCredentials.ConsumerKey, appSettingsCredentials.ConsumerSecret);

			// -- artist/details
			var artist = Api<Artist>.Create
				.WithArtistId(Convert.ToInt32(s))
				.Please();

			Console.WriteLine("Artist \"{0}\" selected", artist.Name);
			Console.WriteLine("Website url is {0}", artist.Url);
			Console.WriteLine();


			// -- artist/toptracks
			var artistTopTracks = Api<ArtistTopTracks>
				.Create
				.WithArtistId(Convert.ToInt32(s))
				.Please();

			Console.WriteLine("Top Track: {0}", artistTopTracks.Tracks.FirstOrDefault().Title);
			Console.WriteLine();

			// -- artist/browse
			const string searchValue = "Radio";
			var artistBrowse = Api<ArtistBrowse>
				.Create
				.WithLetter(searchValue)
				.Please();

			Console.WriteLine("Browse on \"{0}\" returns: {1}", searchValue, artistBrowse.Artists.FirstOrDefault().Name);
			Console.WriteLine();

			// -- artist/search
			var artistSearch = Api<ArtistSearch>.Create
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.Please();

			Console.WriteLine("Artist Search on \"{0}\" returns: {1} items", searchValue, artistSearch.TotalItems);
			Console.WriteLine();

			// -- artist/search parallel 

			var requestor = new FluentApiRequestor<ArtistSearch>(() => new FluentApi<ArtistSearch>(new RequestCoordinator(new GzipHttpClient(), new UrlSigner(), new AppSettingsCredentials(), new ApiUri())));

			Parallel.For(1, 10, i =>
			{
				var result = requestor.Request(x =>
				{
					x.WithQuery("keane");
					x.WithPageNumber(i);
					x.WithPageSize(1);
				});
				Console.WriteLine(i + " >> " + result.Results.First().Artist.Name);
			});

			// -- release/search
			var releaseSearch = Api<ReleaseSearch>.Create
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.Please();

			Console.WriteLine("Release search on \"{0}\" returns: {1} items", searchValue, releaseSearch.TotalItems);
			Console.WriteLine();

			// -- Debug uri
			string currentUri = Api<ReleaseSearch>.Create.WithQuery("Test").EndpointUrl;
			Console.WriteLine("Release search hits: {0}", currentUri);

			// -- async get (async post not implemented yet)
			Api<ReleaseSearch>.Create
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.PleaseAsync(x =>
				{
					Console.WriteLine("Async Release search on \"{0}\" returns: {1} items", "Radio", x.TotalItems);
					Console.WriteLine();
				});

			try
			{
				// -- Deliberate error response
				Console.WriteLine("Trying artist/details without artistId parameter...");
				Api<Artist>.Create.Please();
			}
			catch (ApiException ex)
			{
				Console.WriteLine("{0} : {1}", ex, ex.Message);
			}

			try
			{
				// -- Deliberate unauthorized response
				Console.WriteLine("Trying user/locker without any credentials...");
				Api<Locker>.Create.Please();
			}
			catch (ApiException ex)
			{
				Console.WriteLine("{0} : {1}", ex, ex.Message);
			}

			Console.ReadKey();
		}
	}
}
