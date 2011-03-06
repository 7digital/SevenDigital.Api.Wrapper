using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Repository;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistReleasesTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ArtistReleases artist = new FluentApi<ArtistReleases>(httpGetResolver)
				.WithParameter("artistId", "1")
				.WithParameter("country", "GB")
				.Resolve();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Releases.Count, Is.GreaterThan(0));
			Assert.That(artist.Releases.FirstOrDefault().Artist.Name, Is.EqualTo("Keane"));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ArtistReleases artistBrowse = new FluentApi<ArtistReleases>(httpGetResolver)
				.WithParameter("artistId", "1")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Resolve();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}