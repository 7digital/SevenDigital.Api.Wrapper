using NUnit.Framework;
using SevenDigital.Api.Schema.TrackEndpoint;
using SevenDigital.Api.Wrapper.Extensions.Get;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TrackEndpoint
{
	[TestFixture]
	public class TrackDetailsTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			Track track = Api<Track>.Get
				.ForTrackId(12345)
				.Please();

			Assert.That(track, Is.Not.Null);
			Assert.That(track.Title, Is.EqualTo("I Love You"));
			Assert.That(track.ArtistIdParameter.Name, Is.EqualTo("The Dandy Warhols"));
		}
	}
}