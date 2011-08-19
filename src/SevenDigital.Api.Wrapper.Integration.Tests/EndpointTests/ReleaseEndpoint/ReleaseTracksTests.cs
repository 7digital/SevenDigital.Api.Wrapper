using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Extensions.Get;

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
			Assert.That(releaseTracks.Tracks.FirstOrDefault().Price.IsAvailable, Is.True);
		}

		[Test]
		public void can_determine_if_a_track_is_free()
		{

			var releaseTracks = Api<ReleaseTracks>.Get
				.ForReleaseId(394123)
				.Please();

			Assert.That(releaseTracks, Is.Not.Null);
			Assert.That(releaseTracks.Tracks.Count, Is.EqualTo(1));
			Assert.That(releaseTracks.Tracks.FirstOrDefault().Price.IsFree, Is.True);
		}

		[Test]
		public void can_determine_if_a_track_is_available_separately()
		{

			var releaseTracks = Api<ReleaseTracks>.Get
				.ForReleaseId(1193196)
				.Please();

			Assert.That(releaseTracks, Is.Not.Null);
			Assert.That(releaseTracks.Tracks.Count, Is.EqualTo(8));
			Assert.That(releaseTracks.Tracks.FirstOrDefault().Price.IsAvailable, Is.False);
		}
	}
}