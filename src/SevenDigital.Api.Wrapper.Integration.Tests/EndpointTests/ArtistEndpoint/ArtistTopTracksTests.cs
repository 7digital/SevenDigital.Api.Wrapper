using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Schema.ArtistEndpoint;

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
				.MakeRequest()
				.WithParameter("artistId", "1")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{
			var artist = Api<ArtistTopTracks>
				.Create
				.MakeRequest()
				.WithArtistId(1)
				.WithParameter("country", "GB")
				.Please();
			
			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_handle_pagingerror_with_paging()
		{
			var ex = Assert.Throws<InputParameterException>(() =>
				new FluentApi<ArtistTopTracks>()
					.MakeRequest()
					.WithParameter("artistId", "1")
					.WithParameter("page", "2")
					.WithParameter("pageSize", "10")
					.Please());

			Assert.That(ex.ResponseBody, Is.Not.Null);
			Assert.That(ex.ErrorCode, Is.EqualTo(ErrorCode.ParameterOutOfAllowableRange));
			Assert.That(ex.Message, Is.EqualTo("Requested page out of range"));
		}
	}
}