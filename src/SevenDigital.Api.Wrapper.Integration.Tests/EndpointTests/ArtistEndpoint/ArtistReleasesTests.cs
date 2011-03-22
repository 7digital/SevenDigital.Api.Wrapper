using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistReleasesTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			ArtistReleases artist = new FluentApi<ArtistReleases>()
				.WithParameter("artistId", "1")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Releases.Count, Is.GreaterThan(0));
			Assert.That(artist.Releases.FirstOrDefault().Artist.Name, Is.EqualTo("Keane"));
		}

		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{
			var artist = (ArtistReleases) Api<ArtistReleases>
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
			ArtistReleases artistBrowse = new FluentApi<ArtistReleases>()
				.WithParameter("artistId", "1")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}