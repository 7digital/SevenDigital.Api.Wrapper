using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Tags;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Tags
{
	[TestFixture]
	public class ReleaseTagsTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<ReleaseTags>.Create
				.WithParameter("releaseid", "155408");
			var tags = await request.Please();

			Assert.That(tags, Is.Not.Null);
			Assert.That(tags.TagList.Count, Is.GreaterThan(0));
			Assert.That(tags.TagList.FirstOrDefault().Id, Is.Not.Empty);
			Assert.That(tags.TagList.Where(x => x.Id == "rock").FirstOrDefault().Text, Is.EqualTo("rock"));

		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<ReleaseTags>.Create
				.WithParameter("releaseid", "155408")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "1");
			ReleaseTags releaseTags = await request.Please();

			Assert.That(releaseTags, Is.Not.Null);
			Assert.That(releaseTags.Page, Is.EqualTo(2));
			Assert.That(releaseTags.PageSize, Is.EqualTo(1));
		}
	}
}