using NUnit.Framework;
using SevenDigital.Api.Schema.Artists;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistSearchTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = new FluentApi<ArtistSearch>()
				.WithParameter("q", "pink")
				.WithParameter("country", "GB");
			var artist = await request.Please();

			Assert.That(artist, Is.Not.Null);
		}

		[Test]
		public async void Can_do_similar_to_browse()
		{
			var request = Api<ArtistSearch>
				.Create
				.WithQuery("radiohe")
				.WithParameter("sort", "popularity+desc");
			var artist = await request.Please();

			Assert.That(artist, Is.Not.Null);
			
		}

		[Test]
		public async void Can_hit_endpoint_with_fluent_interface()
		{
			var request = Api<ArtistSearch>
				.Create
				.WithQuery("pink")
				.WithParameter("country", "GB");
			var artistSearch = await request.Please();

			Assert.That(artistSearch, Is.Not.Null);
			
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<ArtistSearch>.Create
				.WithParameter("q", "pink")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20");
			var artistBrowse = await request.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}

		[Test]
		public async void Can_get_multiple_results()
		{
			var request = Api<ArtistSearch>.Create
				.WithParameter("q", "pink")
				.WithParameter("page", "1")
				.WithParameter("pageSize", "20");
			var artistSearch = await request.Please();

			Assert.That(artistSearch.Results.Count, Is.GreaterThan(1));
		}

		[Test]
		public async void Can_get_multiple_results_with_new_FluentApi_overload() 
		{
			var request = new FluentApi<ArtistSearch>(new AppSettingsCredentials(), new ApiUri())
				.ForShop(34)
				.WithQuery("pink")
				.WithPageNumber(1)
				.WithPageSize(20);
			var artistSearch = await request.Please();

			Assert.That(artistSearch.Results.Count, Is.GreaterThan(1));
		}
	}
}