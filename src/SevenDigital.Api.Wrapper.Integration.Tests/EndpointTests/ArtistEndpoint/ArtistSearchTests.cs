using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Extensions;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistSearchTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			ArtistSearch artist = new FluentApi<ArtistSearch>()
				.WithParameter("q", "pink")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Results.Artists.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_do_similar_to_browse()
		{
			var artist = Api<ArtistSearch>
							.Get
							.WithQuery("radiohe")
							.WithParameter("sort","popularity+desc")
							.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Results.Artists.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{

			ArtistSearch artistSearch = Api<ArtistSearch>
				.Get
				.WithQuery("pink")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artistSearch, Is.Not.Null);
			Assert.That(artistSearch.Results.Artists.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			ArtistSearch artistBrowse = Api<ArtistSearch>.Get
				.WithParameter("q", "pink")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}