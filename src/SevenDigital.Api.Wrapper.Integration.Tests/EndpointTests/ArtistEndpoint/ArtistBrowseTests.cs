using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	public class ArtistBrowseTests
	{
		[Test]
		public async void Can_hit_endpoint_with_fluent_interface()
		{
			var request = Api<ArtistBrowse>.Create
				.WithLetter("radio");
			var artistBrowse = await request.Please();
			
			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(1));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(10));
			Assert.That(artistBrowse.Artists.Count, Is.GreaterThan(0));
		}

		[Test]
		public async void Can_hit_endpoint_with_fluent_interface_with_paging()
		{
			var request = Api<ArtistBrowse>
				.Create
				.WithLetter("radio")
				.WithPageNumber(2)
				.WithPageSize(20);
			var artistBrowse = await request.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}