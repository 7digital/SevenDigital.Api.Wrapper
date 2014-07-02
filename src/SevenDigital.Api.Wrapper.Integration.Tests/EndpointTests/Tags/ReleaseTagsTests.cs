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
			const string ExpectedTagId = "pop";

			var request = Api<ReleaseTags>.Create
				.WithParameter("releaseid", "3070977");
			var releaseTags = await request.Please();

			Assert.That(releaseTags, Is.Not.Null);
			Assert.That(releaseTags.TagList, Is.Not.Null);
			Assert.That(releaseTags.TagList.Count, Is.GreaterThan(0));

			Assert.That(releaseTags.TagList[0].Id, Is.Not.Empty);

			var expectedTag = releaseTags.TagList.First(x => x.Id == ExpectedTagId);
			Assert.That(expectedTag.Text, Is.EqualTo(ExpectedTagId));
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<ReleaseTags>.Create
				.WithParameter("releaseid", "3070977")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "1");
			var releaseTags = await request.Please();

			Assert.That(releaseTags, Is.Not.Null);
			Assert.That(releaseTags.Page, Is.EqualTo(2));
			Assert.That(releaseTags.PageSize, Is.EqualTo(1));
		}
	}
}