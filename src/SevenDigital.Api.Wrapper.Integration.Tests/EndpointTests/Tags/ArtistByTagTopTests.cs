using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Schema.Tags;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Tags
{
	[TestFixture]
	public class ArtistByTagTopTests
	{
		private const string Tags = "rock,pop";

		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<ArtistByTagTop>.Create
				.WithParameter("tags", Tags);
			var tags = await request.Please();

			Assert.That(tags, Is.Not.Null);
			Assert.That(tags.TaggedArtists.Count, Is.GreaterThan(0));
			Assert.That(tags.Type, Is.EqualTo(ItemType.artist));
			Assert.That(tags.TaggedArtists.FirstOrDefault().Artist.Name, Is.Not.Empty);
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<ArtistByTagTop>.Create
				.WithParameter("tags", Tags)
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20");
			var artistBrowse = await request.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}