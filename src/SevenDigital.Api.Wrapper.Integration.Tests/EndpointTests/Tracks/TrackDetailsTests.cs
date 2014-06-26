using NUnit.Framework;
using SevenDigital.Api.Schema.Tracks;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Tracks
{
	[TestFixture]
	public class TrackDetailsTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<Track>.Create
				.ForTrackId(12345);
			var track = await request.Please();

			Assert.That(track, Is.Not.Null);
			Assert.That(track.Title, Is.EqualTo("I Love You"));
			Assert.That(track.Artist.Name, Is.EqualTo("The Dandy Warhols"));
		}
	}
}