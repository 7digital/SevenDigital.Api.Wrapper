using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	public class ArtistReleasesTests
	{

		[Test]
		public async void Can_hit_endpoint_with_fluent_interface()
		{
			var request = Api<ArtistReleases>
				.Create
				.WithArtistId(1);
			var artistReleases = await request.Please();

			Assert.That(artistReleases, Is.Not.Null);
			Assert.That(artistReleases.Releases.Count, Is.GreaterThan(0));
			Assert.That(artistReleases.Releases.FirstOrDefault().Artist.Name, Is.EqualTo("Keane"));
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<ArtistReleases>
				.Create
				.WithPageNumber(2)
				.WithPageSize(20)
				.WithParameter("artistId", "1");
			var artistBrowse = await request.Please();
			
			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}