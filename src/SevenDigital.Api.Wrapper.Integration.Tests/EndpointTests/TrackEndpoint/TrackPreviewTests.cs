using NUnit.Framework;
using SevenDigital.Api.Schema.TrackEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TrackEndpoint
{
	[TestFixture]
	public class TrackPreviewTests
	{
		[Test, Ignore("Waiting for API fix to reinstate XML declaration")]
		public void Can_hit_endpoint_with_redirect_false()
		{
			TrackPreview track = Api<TrackPreview>.Create
				.WithParameter("trackid", "123")
				.WithParameter("redirect", "false")
				.Please();

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