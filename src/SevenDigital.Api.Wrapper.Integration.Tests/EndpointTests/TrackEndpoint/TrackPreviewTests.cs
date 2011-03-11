using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Schema.TrackEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TrackEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class TrackPreviewTests
	{
		[Test]
		public void Can_hit_endpoint_with_redirect_false()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());
			TrackPreview track = new FluentApi<TrackPreview>(httpGetResolver)
				.WithParameter("trackid", "123")
				.WithParameter("redirect", "false")
				.Resolve();

			Assert.That(track, Is.Not.Null);
			Assert.That(track.Url, Is.EqualTo("http://previews.7digital.com/clips/34/123.clip.mp3"));
		}

		// TODO
		[Test, Ignore("Need to implement this")]
		public void Can_hit_endpoint_with_redirect_true_and_have_the_output_as_a_stream()
		{
			Assert.Fail("Not implemented yet");
		}
	}
}