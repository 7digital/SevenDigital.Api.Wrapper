using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Extensions.Get;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistTopTracksTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			ArtistIdParameterTopTracks artistIdParameter = new FluentApi<ArtistIdParameterTopTracks>()
				.WithParameter("artistId", "1")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artistIdParameter, Is.Not.Null);
			Assert.That(artistIdParameter.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{
			var artist = (ArtistIdParameterTopTracks)Api<ArtistIdParameterTopTracks>
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
				new FluentApi<ArtistIdParameterTopTracks>()
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