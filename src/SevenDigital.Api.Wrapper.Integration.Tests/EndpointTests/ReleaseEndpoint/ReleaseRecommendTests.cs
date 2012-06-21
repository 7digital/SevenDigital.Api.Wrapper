using NUnit.Framework;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ReleaseEndpoint
{
	[TestFixture]
	public class ReleaseRecommendTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			ReleaseRecommend release = Api<ReleaseRecommend>.Create
				.WithParameter("releaseId", "155408")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.RecommendedItems.Count, Is.GreaterThan(0));
		}
	}
}