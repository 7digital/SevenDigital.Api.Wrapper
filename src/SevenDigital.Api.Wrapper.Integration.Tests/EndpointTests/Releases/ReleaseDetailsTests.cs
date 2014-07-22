using NUnit.Framework;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Releases
{
	[TestFixture]
	[Category("Integration")]
	public class ReleaseDetailsTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<Release>.Create
				.ForReleaseId(1685647)
				.WithParameter("country", "GB");
			var release = await request.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.Title, Is.EqualTo("Strangeland"));
			Assert.That(release.Artist.Name, Is.EqualTo("Keane"));
			Assert.That(release.TrackCount, Is.EqualTo(12));
		}

	}
}