using System;
using System.Linq;
using System.Threading.Tasks;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.LockerEndpoint;

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

			// console apps can't have an async main, so we have to deal with that
			var task = use7DigitalApi(artistId);
			task.Wait();

			Console.ReadKey();
		}

		private static async Task use7DigitalApi(int artistId)
		{

			// -- artist/details
			var artist = await Api<Artist>.Create
				.WithArtistId(artistId)
				.Please();

			Console.WriteLine("Artist \"{0}\" selected", artist.Name);
			Console.WriteLine("Website url is {0}", artist.Url);
			Console.WriteLine();


			// -- artist/toptracks
			var artistTopTracks = await Api<ArtistTopTracks>
				.Create
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
			var artistSearch = await Api<ArtistSearch>.Create
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.Please();

			Console.WriteLine("Artist Search on \"{0}\" returns: {1} items", searchValue, artistSearch.TotalItems);
			Console.WriteLine();

			// -- release/search
			var releaseSearch = await Api<ReleaseSearch>.Create
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.Please();

			Console.WriteLine("Release search on \"{0}\" returns: {1} items", searchValue, releaseSearch.TotalItems);
			Console.WriteLine();

			// -- Debug uri
			string currentUri = Api<ReleaseSearch>.Create.WithQuery("Test").EndpointUrl;
			Console.WriteLine("Release search hits: {0}", currentUri);

			try
			{
				// -- Deliberate error response
				Console.WriteLine("Trying artist/details without artistId parameter...");
				await Api<Artist>.Create.Please();
			}

			catch (ApiResponseException ex)
			{
				Console.WriteLine("{0} : {1}", ex, ex.Message);
			}

			try
			{
				// -- Deliberate unauthorized response
				Console.WriteLine("Trying user/locker without any credentials...");
				await Api<Locker>.Create.Please();
			}
			catch (ApiResponseException ex)
			{
				Console.WriteLine("{0} : {1}", ex, ex.Message);
			}
		}
	}
}
