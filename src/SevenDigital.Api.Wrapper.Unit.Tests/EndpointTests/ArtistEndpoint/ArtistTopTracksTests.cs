using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistTopTracksTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ArtistTopTracks artist = new FluentApi<ArtistTopTracks>(httpGetResolver)
				.WithParameter("artistId", "1")
				.WithParameter("country", "GB")
				.Resolve();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ArtistTopTracks artist = new FluentApi<ArtistTopTracks>(httpGetResolver)
				.WithParameter("artistId", "1")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "10")
				.Resolve();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Page, Is.EqualTo(2));
			Assert.That(artist.PageSize, Is.EqualTo(10));
		}

		[Test]
		public void Can_handle_pagingerror_with_paging()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			try
			{
				new FluentApi<ArtistTopTracks>(httpGetResolver)
					.WithParameter("artistId", "1")
					.WithParameter("page", "2")
					.WithParameter("pageSize", "10")
					.Resolve();
			} 
			catch(ApiXmlException ex)
			{
				Assert.That(ex.Error, Is.Not.Null);
				Assert.That(ex.Error.Code, Is.EqualTo(1003));
				Assert.That(ex.Error.ErrorMessage, Is.EqualTo("Requested page out of range"));
			}
		}
	}
}