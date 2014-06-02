using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistTopTracksTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var artistTopTracks = new FluentApi<ArtistTopTracks>()
				.WithParameter("artistId", "1")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artistTopTracks, Is.Not.Null);
			Assert.That(artistTopTracks.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{
			var artistTopTracks = Api<ArtistTopTracks>
				.Create
				.WithArtistId(1)
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artistTopTracks, Is.Not.Null);
			Assert.That(artistTopTracks.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_handle_out_of_range_request()
		{
			var artistTopTracks = new FluentApi<ArtistTopTracks>()
				.WithParameter("artistId", "1")
				.WithParameter("page", "100")
				.WithParameter("pageSize", "10")
				.Please();

			Assert.That(artistTopTracks, Is.Not.Null);
			Assert.That(artistTopTracks.Tracks.Count, Is.EqualTo(0));
		}
	}
}