using NUnit.Framework;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Releases
{
	[TestFixture]
	public class ReleaseRecommendTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<ReleaseRecommend>.Create
				.ForReleaseId(155408)
				.WithParameter("country", "GB");
			var release = await request.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.RecommendedItems.Count, Is.GreaterThan(0));
		}
	}
}