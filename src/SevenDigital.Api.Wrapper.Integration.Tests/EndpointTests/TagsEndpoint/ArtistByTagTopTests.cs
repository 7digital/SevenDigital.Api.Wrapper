using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Schema.Tags;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TagsEndpoint
{
	[TestFixture]
	public class ArtistByTagTopTests
	{
		[Test]
		public void Can_hit_endpoint()
		{

			ArtistByTagTop tags = Api<ArtistByTagTop>.Get
				.WithParameter("tags", "rock,pop,2000s")
				.Please();

			Assert.That(tags, Is.Not.Null);
			Assert.That(tags.TaggedArtists.Count, Is.GreaterThan(0));
			Assert.That(tags.Type, Is.EqualTo(ItemType.artist));
			Assert.That(tags.TaggedArtists.FirstOrDefault().ArtistIdParameter.Name, Is.Not.Empty);
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{

			ArtistByTagTop artistBrowse = Api<ArtistByTagTop>.Get
				.WithParameter("tags", "rock,pop,2000s")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}