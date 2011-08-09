using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Extensions;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistTopTracksTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			ArtistTopTracks artist = new FluentApi<ArtistTopTracks>()
				.WithParameter("artistId", "1")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{
			var artist = (ArtistTopTracks)Api<ArtistTopTracks>
								.Get
								.WithArtistId(1)
								.WithParameter("country", "GB")
								.Please();
			
			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_handle_pagingerror_with_paging()
		{
			try
			{
				new FluentApi<ArtistTopTracks>()
					.WithParameter("artistId", "1")
					.WithParameter("page", "2")
					.WithParameter("pageSize", "10")
					.Please();
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