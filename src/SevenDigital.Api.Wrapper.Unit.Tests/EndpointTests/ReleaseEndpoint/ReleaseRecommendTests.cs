using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointTests.ReleaseEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ReleaseRecommendTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ReleaseRecommend release = new FluentApi<ReleaseRecommend>(httpGetResolver)
				.WithParameter("releaseId", "155408")
				.WithParameter("country", "GB")
				.Resolve();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.RecommendedItems.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ReleaseChart artistBrowse = new FluentApi<ReleaseChart>(httpGetResolver)
				.WithParameter("releaseId", "155408")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Resolve();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}