using System;
using System.Linq;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper.ExampleUsage {
	class Program {
		static void Main(string[] args) {
			string s = args[0];

			try {
				
				// -- artist/details
				Artist artist = new FluentApi<Artist>()
									.WithParameter("artistId", s) 
									.Resolve();

				Console.WriteLine("Artist \"{0}\" selected", artist.Name);
				Console.WriteLine("Website url is {0}", artist.Url);
				Console.WriteLine();

				// -- artist/toptracks
				ArtistTopTracks artistTopTracks = new FluentApi<ArtistTopTracks>()
														.WithParameter("artistId", s)
														.Resolve();

				Console.WriteLine("Top Track: {0}", artistTopTracks.Tracks.FirstOrDefault().Title);
				Console.WriteLine();

				// -- artist/browse
				const string searchValue = "Radioh";
				ArtistBrowse artistBrowse = new FluentApi<ArtistBrowse>()
														.WithParameter("letter", searchValue)
														.Resolve();

				Console.WriteLine("Browse on \"{0}\" returns: {1}", searchValue, artistBrowse.Artists.FirstOrDefault().Name);
				Console.WriteLine();

				// -- Deliberate error response
				Console.WriteLine("Trying artist/details without artistId parameter...");
				new FluentApi<Artist>().Resolve();

			} catch(ApiXmlException ex) {

				Console.WriteLine("{0} : {1}", ex.Error.Code, ex.Error.ErrorMessage);
			}
			Console.ReadKey();
		}
	}
}
