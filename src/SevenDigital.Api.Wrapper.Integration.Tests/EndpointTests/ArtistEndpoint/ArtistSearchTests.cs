using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;

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
		}

		[Test]
		public void Can_do_similar_to_browse()
		{
			var artist = Api<ArtistSearch>
				.Create
				.WithQuery("radiohe")
				.WithParameter("sort","popularity+desc")
				.Please();

			Assert.That(artist, Is.Not.Null);
			
		}

		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{

			ArtistSearch artistSearch = Api<ArtistSearch>
				.Create
				.WithQuery("pink")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artistSearch, Is.Not.Null);
			
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			ArtistSearch artistBrowse = Api<ArtistSearch>.Create
				.WithParameter("q", "pink")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}

		[Test]
		public void Can_get_multiple_results()
		{
			ArtistSearch artistSearch = Api<ArtistSearch>.Create
				.WithParameter("q", "pink")
				.WithParameter("page", "1")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistSearch.Results.Count, Is.GreaterThan(1));
		}

		[Test]
		public void Can_get_multiple_results_with_new_FluentApi_overload() 
		{
			var artistSearch = new FluentApi<ArtistSearch>(new AppSettingsCredentials(), new ApiUri())
				.ForShop(34)
				.WithQuery("pink")
				.WithPageNumber(1)
				.WithPageSize(20)
				.Please();

			Assert.That(artistSearch.Results.Count, Is.GreaterThan(1));
		}
	}
}