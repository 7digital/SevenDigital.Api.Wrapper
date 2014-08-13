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
			var request = Api<Track>.Create.ForTrackId(12345);
			var track = await request.Please();

			Assert.That(track, Is.Not.Null);
			Assert.That(track.Title, Is.EqualTo("I Love You"));
			Assert.That(track.TrackNumber, Is.EqualTo(5));
			Assert.That(track.Number, Is.EqualTo(5));
			Assert.That(track.DiscNumber, Is.EqualTo(1));
			
			Assert.That(track.Artist.Id, Is.EqualTo(437));
			Assert.That(track.Artist.Name, Is.EqualTo("The Dandy Warhols"));
			Assert.That(track.Artist.AppearsAs, Is.EqualTo("The Dandy Warhols"));
		}
	}
}