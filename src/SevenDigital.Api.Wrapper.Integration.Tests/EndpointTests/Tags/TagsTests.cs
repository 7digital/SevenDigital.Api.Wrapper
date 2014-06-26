using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Tags;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Tags
{
	[TestFixture]
	public class TagsTests 
	{
		[Test]
		public async void Can_hit_endpoint() 
		{
            var tags = await Api<TagsResponse>.Create.Please();

			Assert.That(tags, Is.Not.Null);
			Assert.That(tags.TagList.Count, Is.GreaterThan(0));
			Assert.That(tags.TagList.FirstOrDefault().Id, Is.Not.Empty);
			Assert.That(tags.TagList.Where(x=>x.Id == "rock").FirstOrDefault().Text, Is.EqualTo("rock"));
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
            var request = Api<TagsResponse>.Create
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20");
			var tags = await request.Please();

			Assert.That(tags, Is.Not.Null);
			Assert.That(tags.Page, Is.EqualTo(2));
			Assert.That(tags.PageSize, Is.EqualTo(20));
		}
	}
}