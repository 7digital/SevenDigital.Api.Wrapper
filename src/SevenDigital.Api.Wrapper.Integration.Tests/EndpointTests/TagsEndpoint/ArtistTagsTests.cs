using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Tags;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TagsEndpoint
{
	[TestFixture]
	public class ArtistTagsTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			ArtistTags tags = Api<ArtistTags>.Create.MakeRequest()
									.WithParameter("artistId", "1")
									.Please();

			Assert.That(tags, Is.Not.Null);
			Assert.That(tags.TagList.Count, Is.GreaterThan(0));
			Assert.That(tags.TagList.FirstOrDefault().Id, Is.Not.Empty);
			Assert.That(tags.TagList.Where(x => x.Id == "rock").FirstOrDefault().Text, Is.EqualTo("rock"));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			ArtistTags artistBrowse = Api<ArtistTags>.Create
				.MakeRequest()
				.WithParameter("artistId", "1")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "1")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(1));
		}
	}
}