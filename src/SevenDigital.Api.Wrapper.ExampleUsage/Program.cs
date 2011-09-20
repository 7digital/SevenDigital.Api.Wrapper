using System;
using System.Linq;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.LockerEndpoint;

namespace SevenDigital.Api.Wrapper.ExampleUsage {
	class Program {
		static void Main(string[] args) {
			string s = args[0];

			var appSettingsCredentials = new AppSettingsCredentials();
			Console.WriteLine("Using creds: {0} - {1}", appSettingsCredentials.ConsumerKey, appSettingsCredentials.ConsumerSecret);

			// -- artist/details
			var artist = Api<Artist>.Get
				.WithArtistId(Convert.ToInt32(s))
				.Please();

			Console.WriteLine("Artist \"{0}\" selected", artist.Name);
			Console.WriteLine("Website url is {0}", artist.Url);
			Console.WriteLine();


			// -- artist/toptracks
			var artistTopTracks = Api<ArtistTopTracks>
				.Get
				.WithArtistId(Convert.ToInt32(s))
				.Please();

			Console.WriteLine("Top Track: {0}", artistTopTracks.Tracks.FirstOrDefault().Title);
			Console.WriteLine();
			
			// -- artist/browse
			const string searchValue = "Radio";
			var artistBrowse = Api<ArtistBrowse>
				.Get
				.WithLetter(searchValue)
				.Please();

			Console.WriteLine("Browse on \"{0}\" returns: {1}", searchValue, artistBrowse.Artists.FirstOrDefault().Name);
			Console.WriteLine();

			// -- artist/search
			var artistSearch = Api<ArtistSearch>.Get
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.Please();

			Console.WriteLine("Artist Search on \"{0}\" returns: {1} items", searchValue, artistSearch.TotalItems);
			Console.WriteLine();

			// -- release/search
			var releaseSearch = Api<ReleaseSearch>.Get
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.Please();

			Console.WriteLine("Release search on \"{0}\" returns: {1} items", searchValue, releaseSearch.TotalItems);
			Console.WriteLine();

			// -- Debug uri
			string currentUri = Api<ReleaseSearch>.Get.WithQuery("Test").GetCurrentUri();
			Console.WriteLine("Release search hits: {0}", currentUri);

			// -- async get (async post not implemented yet)
			Api<ReleaseSearch>.Get
				.WithQuery(searchValue)
				.WithPageNumber(1)
				.WithPageSize(10)
				.PleaseAsync(x => {
					Console.WriteLine("Async Release search on \"{0}\" returns: {1} items", "Radio", x.TotalItems);
					Console.WriteLine();
				 });

			try {
				// -- Deliberate error response
				Console.WriteLine("Trying artist/details without artistId parameter...");
				Api<Artist>.Get.Please();
			} catch (ApiXmlException ex) {
				Console.WriteLine("{0} : {1}", ex.Error.Code, ex.Error.ErrorMessage);
			}

			try {
				// -- Deliberate unauthorized response
				Console.WriteLine("Trying user/locker without any credentials...");
				Api<Locker>.Get.Please();
			} catch (ApiXmlException ex) {
				Console.WriteLine("{0} : {1}", ex.Error.Code, ex.Error.ErrorMessage);
			}

			Console.ReadKey();
		}
	}


}
