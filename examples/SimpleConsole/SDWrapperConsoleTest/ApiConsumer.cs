using System;
using System.Threading.Tasks;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Schema.Releases;
using SevenDigital.Api.Wrapper;

namespace SDWrapperConsoleTest
{
	public class ApiConsumer
	{
		private readonly IApi _api;

		public ApiConsumer(IApi api)
		{
			_api = api;
		}

		public async Task ShowReleaseCharts()
		{
			var request = _api.Create<ReleaseChart>();
			var chart = await request.Please();

			Console.WriteLine("Retreieved a chart containing {0} releases", chart.ChartItems.Count);
			foreach (var chartItem in chart.ChartItems)
			{
				Console.WriteLine("{0}: '{1}' by {2}",
					chartItem.Position, chartItem.Release.Title, chartItem.Release.Artist.Name);
			}

			Console.WriteLine();

			foreach (var chartItem in chart.ChartItems)
			{
				var artist = chartItem.Release.Artist;
				Console.WriteLine("Also by {0}", artist.Name);
				await ShowAlsoByArtist(artist.Id);
				Console.WriteLine();
			}
		}

		private async Task ShowAlsoByArtist(int artistId)
		{
			var request = _api.Create<ArtistReleases>()
				.WithArtistId(artistId)
				.WithPageSize(5);

			var artistReleases = await request.Please();

			foreach (var release in artistReleases.Releases)
			{
				Console.WriteLine("{0} {1} {2}", release.Title, release.Version, release.Type);
			}
		}
	}
}