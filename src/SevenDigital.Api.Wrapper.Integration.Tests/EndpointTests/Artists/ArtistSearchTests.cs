using NUnit.Framework;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Artists
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistSearchTests
	{
		private FluentApi<ArtistSearch> _fluentApi;

		[SetUp]
		public void SetUp()
		{
			_fluentApi = new FluentApi<ArtistSearch>(new HttpClientMediator(), new RequestBuilder(new ApiUri(), new AppSettingsCredentials()), new ResponseParser(new ApiResponseDetector()));
		}

		[Test]
		public async void Can_hit_endpoint()
		{
			var request = _fluentApi
				.WithParameter("q", "pink")
				.WithParameter("country", "GB");
			var artist = await request.Please();

			Assert.That(artist, Is.Not.Null);
		}
		
		[Test]
		public async void Can_get_multiple_results()
		{
			var request = _fluentApi
				.ForShop(34)
				.WithQuery("pink")
				.WithPageNumber(1)
				.WithPageSize(20);
			var artistSearch = await request.Please();

			Assert.That(artistSearch.Results.Count, Is.GreaterThan(1));
		}

		[Test]
		public async void Can_hit_endpoint_with_static_interface()
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
		public async void Can_get_multiple_results_with_static_interface()
		{
			var request = Api<ArtistSearch>.Create
				.WithParameter("q", "pink")
				.WithParameter("page", "1")
				.WithParameter("pageSize", "20");
			var artistSearch = await request.Please();

			Assert.That(artistSearch.Results.Count, Is.GreaterThan(1));
		}
	}
}