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
				.ForReleaseId(3070977);
			var releaseTracks = await request.Please();

			Assert.That(releaseTracks, Is.Not.Null);
			Assert.That(releaseTracks.Tracks.Count, Is.EqualTo(10));
			Assert.That(releaseTracks.Tracks.First().Title, Is.EqualTo("Burning"));
			Assert.That(releaseTracks.Tracks.First().Price.Status, Is.EqualTo(PriceStatus.Available));
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