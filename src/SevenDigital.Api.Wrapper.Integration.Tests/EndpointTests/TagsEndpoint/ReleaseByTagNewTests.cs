using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Schema.Tags;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TagsEndpoint
{
	[TestFixture]
	public class ReleaseByTagNewTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<ReleaseByTagNew>.Create
				.WithParameter("tags", "rock");
			var tags = await request.Please();

			Assert.That(tags, Is.Not.Null);
			Assert.That(tags.TaggedReleases.Count, Is.GreaterThan(0));
			Assert.That(tags.Type, Is.EqualTo(ItemType.release));
			Assert.That(tags.TaggedReleases.FirstOrDefault().Release.Title, Is.Not.Empty);
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<ReleaseByTagNew>.Create
				.WithParameter("tags", "rock")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20");
			var releaseByTag = await request.Please();

			Assert.That(releaseByTag, Is.Not.Null);
			Assert.That(releaseByTag.Page, Is.EqualTo(2));
			Assert.That(releaseByTag.PageSize, Is.EqualTo(20));
		}
	}
}