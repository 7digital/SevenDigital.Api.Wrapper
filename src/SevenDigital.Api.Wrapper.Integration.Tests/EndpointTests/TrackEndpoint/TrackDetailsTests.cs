using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Schema.TrackEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TrackEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class TrackDetailsTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());
			Track track = new FluentApi<Track>(httpGetResolver)
				.WithTrackId(12345)
				.Resolve();

			Assert.That(track, Is.Not.Null);
			Assert.That(track.Title, Is.EqualTo("I Love You"));
			Assert.That(track.Artist.Name, Is.EqualTo("The Dandy Warhols"));
		}
	}
}