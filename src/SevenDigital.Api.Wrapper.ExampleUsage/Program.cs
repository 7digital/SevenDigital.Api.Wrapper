using System;
using System.Linq;
using System.Threading.Tasks;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Schema.Lockers;
using SevenDigital.Api.Schema.Releases;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.ExampleUsage 
{
	class Program 
	{
		static void Main(string[] args) 
		{
			var s = args[0];
			var artistId = Convert.ToInt32(s);

			var appSettingsCredentials = new AppSettingsCredentials();
			Console.WriteLine("Using creds: {0} - {1}", appSettingsCredentials.ConsumerKey, appSettingsCredentials.ConsumerSecret);

			// console apps can't have an async main method, so we have to call an async method 
			var task = Use7DigitalApi(artistId);
			task.Wait();

			Console.ReadKey();
		}

		private static async Task Use7DigitalApi(int artistId)
		{
			var api = new ApiFactory();

			// -- artist/details
			var artist = await api.Create<Artist>()
				.WithArtistId(artistId)
				.Please();

			Console.WriteLine("Artist \"{0}\" selected", artist.Name);
			Console.WriteLine("Website url is {0}", artist.Url);
			Console.WriteLine();


			// -- artist/toptracks
			var artistTopTracks = await api.Create<ArtistTopTracks>()
				.WithArtistId(artistId)
				.Please();

			Console.WriteLine("Top Track: {0}", artistTopTracks.Tracks.FirstOrDefault().Title);
			Console.WriteLine();

			// -- artist/browse
			const string searchValue = "Radio";
			var artistBrowse = await Api<ArtistBrowse>
				.Create
				.WithLetter(searchValue)
				.Please();

			Console.WriteLine("Browse on \"{0}\" returns: {1}", searchValue, artistBrowse.Artists.FirstOrDefault().Name);
			Console.WriteLine();

			// -- artist/search
			var artistSearch = await api.Create<ArtistSearch>()
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.Please();

			Console.WriteLine("Artist Search on \"{0}\" returns: {1} items", searchValue, artistSearch.TotalItems);
			Console.WriteLine();

			// -- release/search
			var releaseSearch = await api.Create<ReleaseSearch>()
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.Please();

			Console.WriteLine("Release search on \"{0}\" returns: {1} items", searchValue, releaseSearch.TotalItems);
			Console.WriteLine();

			await DoParalellSearches(api, searchValue);

			// -- Debug uri
		string currentUri = api.Create<ReleaseSearch>()
				.WithQuery("Test").EndpointUrl;
			Console.WriteLine("Release search hits: {0}", currentUri);

			try
			{
				// -- Deliberate error response
				Console.WriteLine("Trying artist/details without artistId parameter...");
				await api.Create<Artist>().Please();
			}

			catch (ApiResponseException ex)
			{
				Console.WriteLine("{0} : {1}", ex, ex.Message);
			}

			try
			{
				// -- Deliberate unauthorized response
				Console.WriteLine("Trying user/locker without any credentials...");
				await api.Create<Locker>().Please();
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine("{0} : {1}", ex, ex.Message);
			}
		}

		private static async Task DoParalellSearches(IApi api, string searchValue)
		{
			// queue up two requests - 
			// note that "await" is not used here - we get the task not the response
			var page1Task = api.Create<ReleaseSearch>()
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.Please();

			var page2Task = api.Create<ReleaseSearch>()
				.WithQuery(searchValue)
				.WithPageNumber(2)
				.WithPageSize(10)
				.Please();

			// wait for both to complete 
			await Task.WhenAll(new[] {page1Task, page2Task});

			var page1Results = page1Task.Result;
			var page2Results = page2Task.Result;

			Console.WriteLine("Two release searches on \"{0}\" returns: {1} items in page 1 and {2} items in page 2", searchValue,
				page1Results.Results.Count, page2Results.Results.Count);

		}
	}
}
