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
		public void Can_hit_endpoint()
		{

			ReleaseByTagNew tags = Api<ReleaseByTagNew>.Create.MakeRequest()
				.WithParameter("tags", "rock")
				.Please();

			Assert.That(tags, Is.Not.Null);
			Assert.That(tags.TaggedReleases.Count, Is.GreaterThan(0));
			Assert.That(tags.Type, Is.EqualTo(ItemType.release));
			Assert.That(tags.TaggedReleases.FirstOrDefault().Release.Title, Is.Not.Empty);
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{

			ReleaseByTagNew artistBrowse = Api<ReleaseByTagNew>.Create.MakeRequest()
				.WithParameter("tags", "rock")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}