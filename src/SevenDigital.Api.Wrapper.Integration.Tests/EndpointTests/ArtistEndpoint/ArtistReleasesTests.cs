using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	public class ArtistReleasesTests
	{

		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{
			var artist = (ArtistReleases)Api<ArtistReleases>
				.Get
				.WithArtistId(1)
				.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Releases.Count, Is.GreaterThan(0));
			Assert.That(artist.Releases.FirstOrDefault().Artist.Name, Is.EqualTo("Keane"));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			var artistBrowse = (ArtistReleases)Api<ArtistReleases>
				.Get
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