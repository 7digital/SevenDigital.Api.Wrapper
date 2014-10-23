using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Pricing;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Releases
{
	[TestFixture]
	public class ReleaseTracksTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<ReleaseTracks>.Create
				.ForReleaseId(1996067);
			var releaseTracks = await request.Please();

			Assert.That(releaseTracks, Is.Not.Null);
			Assert.That(releaseTracks.Tracks.Count, Is.EqualTo(16));
			Assert.That(releaseTracks.Tracks.First().Title, Is.EqualTo("Never Gonna Give You Up"));
			Assert.That(releaseTracks.Tracks.First().Price.Status, Is.EqualTo(PriceStatus.Available));

            Assert.That(releaseTracks.Tracks.First().Download.Packages[0].Id, Is.EqualTo(2));
            Assert.That(releaseTracks.Tracks.First().Download.Packages[0].Description, Is.EqualTo("standard"));
            Assert.That(releaseTracks.Tracks.First().Download.Packages[0].PriceResponse.CurrencyCode, Is.EqualTo("GBP"));
            Assert.That(releaseTracks.Tracks.First().Download.Packages[0].PriceResponse.SevendigitalPrice, Is.EqualTo(0.99));
            Assert.That(releaseTracks.Tracks.First().Download.Packages[0].PriceResponse.RecommendedRetailPrice, Is.EqualTo(0.99));
            Assert.That(releaseTracks.Tracks.First().Download.Packages[0].Formats[0].Id, Is.EqualTo((17)));
            Assert.That(releaseTracks.Tracks.First().Download.Packages[0].Formats[0].Description, Is.EqualTo("MP3 320"));
		}

		[Test]
		public async void can_determine_if_a_track_is_free()
		{
			var request = Api<ReleaseTracks>.Create
				.ForReleaseId(394123);
			var releaseTracks = await request.Please();

			Assert.That(releaseTracks, Is.Not.Null);
			Assert.That(releaseTracks.Tracks.Count, Is.EqualTo(1));
			Assert.That(releaseTracks.Tracks.First().Price.Status, Is.EqualTo(PriceStatus.Free));
		}

		[Test]
		public async void can_determine_if_a_track_is_available_separately()
		{
			var request = Api<ReleaseTracks>.Create
				.ForReleaseId(1193196);
			var releaseTracks = await request.Please();

			Assert.That(releaseTracks, Is.Not.Null);
			Assert.That(releaseTracks.Tracks.Count, Is.GreaterThanOrEqualTo(1));
			Assert.That(releaseTracks.Tracks.First().Price.Status, Is.EqualTo(PriceStatus.UnAvailable));
		}
	}
}