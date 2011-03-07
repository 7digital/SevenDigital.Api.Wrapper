using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistBrowseTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ArtistBrowse artistBrowse = new FluentApi<ArtistBrowse>(httpGetResolver).WithParameter("letter", "radio").Resolve();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(1));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(10));
			Assert.That(artistBrowse.Artists.Count,Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ArtistBrowse artistBrowse = new FluentApi<ArtistBrowse>(httpGetResolver)
				.WithParameter("letter", "radio")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Resolve();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}