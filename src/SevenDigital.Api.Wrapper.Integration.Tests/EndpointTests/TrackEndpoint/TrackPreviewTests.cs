using NUnit.Framework;
using SevenDigital.Api.Schema.TrackEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TrackEndpoint
{
	[TestFixture]
	public class TrackPreviewTests
	{
		[Test]
		public async void Can_hit_endpoint_with_redirect_false()
		{
			var request = Api<TrackPreview>.Create
				.WithParameter("trackid", "123")
				.WithParameter("redirect", "false");
			var track = await request.Please();

			Assert.That(track, Is.Not.Null);
			Assert.That(track.Url, Is.StringStarting("http://previews.7digital.com/clip"));
		}
	}
}