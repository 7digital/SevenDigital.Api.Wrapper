using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Pricing;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ReleaseEndpoint
{
	[TestFixture]
	public class ReleaseTracksTests
	{
		[Test]
		public void Can_hit_endpoint()
		{

			var releaseTracks = Api<ReleaseTracks>.Get
				.ForReleaseId(155408)
				.Please();

			Assert.That(releaseTracks, Is.Not.Null);
			Assert.That(releaseTracks.Tracks.Count, Is.EqualTo(10));
			Assert.That(releaseTracks.Tracks.FirstOrDefault().Title, Is.EqualTo("Burning"));
			Assert.That(releaseTracks.Tracks.FirstOrDefault().Price.Status, Is.EqualTo(PriceStatus.Available));
		}

		[Test]
		public void can_determine_if_a_track_is_free()
		{

			var releaseTracks = Api<ReleaseTracks>.Get
				.ForReleaseId(394123)
				.Please();

			Assert.That(releaseTracks, Is.Not.Null);
			Assert.That(releaseTracks.Tracks.Count, Is.EqualTo(1));
			Assert.That(releaseTracks.Tracks.FirstOrDefault().Price.Status, Is.EqualTo(PriceStatus.Free));
		}

		[Test]
		public void can_determine_if_a_track_is_available_separately()
		{

			var releaseTracks = Api<ReleaseTracks>.Get
				.ForReleaseId(1193196)
				.Please();

			Assert.That(releaseTracks, Is.Not.Null);
			Assert.That(releaseTracks.Tracks.Count, Is.EqualTo(8));
			Assert.That(releaseTracks.Tracks.FirstOrDefault().Price.Status, Is.EqualTo(PriceStatus.UnAvailable));
		}
	}
}