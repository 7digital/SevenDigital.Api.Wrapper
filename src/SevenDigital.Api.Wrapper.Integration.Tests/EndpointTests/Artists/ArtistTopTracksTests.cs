using NUnit.Framework;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Artists
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistTopTracksTests
	{
		private FluentApi<ArtistTopTracks> _fluentApi;

		[SetUp]
		public void SetUp()
		{
			_fluentApi = new FluentApi<ArtistTopTracks>(new HttpClientMediator(), new RequestBuilder(new ApiUri(), new AppSettingsCredentials()), new ResponseParser(new ApiResponseDetector()));
		}

		[Test]
		public async void Can_hit_endpoint()
		{
			
			var artistTopTracks = await _fluentApi
				.WithParameter("artistId", "1")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artistTopTracks, Is.Not.Null);
			Assert.That(artistTopTracks.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public async void Can_hit_endpoint_with_static_fluent_interface()
		{
			var artistTopTracks = await Api<ArtistTopTracks>
				.Create
				.WithArtistId(1)
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artistTopTracks, Is.Not.Null);
			Assert.That(artistTopTracks.Tracks.Count, Is.GreaterThan(0));
		}

		[Test]
		public async void Can_handle_out_of_range_request()
		{
			var artistTopTracks = await _fluentApi
				.WithParameter("artistId", "1")
				.WithParameter("page", "100")
				.WithParameter("pageSize", "10")
				.Please();

			Assert.That(artistTopTracks, Is.Not.Null);
			Assert.That(artistTopTracks.Tracks.Count, Is.EqualTo(0));
		}
	}
}