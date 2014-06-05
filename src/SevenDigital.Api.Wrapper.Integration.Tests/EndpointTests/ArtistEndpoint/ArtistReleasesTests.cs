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
			var artist = await Api<ArtistReleases>
				.Create
				.WithArtistId(1)
				.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Releases.Count, Is.GreaterThan(0));
			Assert.That(artist.Releases.FirstOrDefault().Artist.Name, Is.EqualTo("Keane"));
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var artistBrowse = await Api<ArtistReleases>
				.Create
				.WithPageNumber(2)
				.WithPageSize(20)
				.WithParameter("artistId","1")
				.Please();
			
			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}