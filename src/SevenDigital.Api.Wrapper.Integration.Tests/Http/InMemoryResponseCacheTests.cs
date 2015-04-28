using System.Net;
using System.Xml.Serialization;
using NUnit.Framework;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Integration.Tests.Http
{
	[XmlRoot("response")]
	public class RawArtistChartResponse
	{
		[XmlElement("chart")]
		public ArtistChart Chart { get; set; }
	}
	
	
	[TestFixture]
	public class InMemoryResponseCacheTests
	{
		private static readonly IResponseCache _cache = new InMemoryResponseCache();

		[Test]
		public async void DataIsCachedWhenCacheIsUsed()
		{
			var response1 = await Api<ArtistChart>
				.Create
				.UsingCache(_cache)
				.Please();

			var response2 = await Api<ArtistChart>
				.Create
				.UsingCache(_cache)
				.Please();

			Assert.That(response2, Is.EqualTo(response1));
		}

		[Test]
		public async void DataIsNotCachedWhenCacheIsNotUsed()
		{
			var response1 = await Api<ArtistChart>
				.Create
				.Please();

			var response2 = await Api<ArtistChart>
				.Create
				.Please();

			Assert.That(response2, Is.Not.EqualTo(response1));
		}

		[Test]
		public async void ResponseIsCachedWhenCacheIsUsed()
		{
			var response1 =  await Api<ArtistChart>
				.Create
				.UsingCache(_cache)
				.Response();

			AssertIsCacheableResponse(response1);

			var response2 = await Api<ArtistChart>
				.Create
				.UsingCache(_cache)
				.Response();

			AssertIsCacheableResponse(response2);
			Assert.That(response2, Is.EqualTo(response1));
		}

		[Test]
		public async void ResponseIsNotCachedWhenCacheIsNotUsed()
		{
			var response1 = await Api<ArtistChart>
				.Create
				.Response();

			AssertIsCacheableResponse(response1);

			var response2 = await Api<ArtistChart>
				.Create
				.Response();

			AssertIsCacheableResponse(response2);
			Assert.That(response2, Is.Not.EqualTo(response1));
		}

		[Test]
		public async void ResponseAsIsCachedWhenCacheIsUsed()
		{
			var response1 = await Api<ArtistChart>
				.Create
				.UsingCache(_cache)
				.ResponseAs<RawArtistChartResponse>();

			var response2 = await Api<ArtistChart>
				.Create
				.UsingCache(_cache)
				.ResponseAs<RawArtistChartResponse>();

			Assert.That(response2, Is.EqualTo(response1));
		}

		[Test]
		public async void ResponseAsIsNotCachedWhenCacheIsNotUsed()
		{
			var response1 = await Api<ArtistChart>
				.Create
				.ResponseAs<RawArtistChartResponse>();

			var response2 = await Api<ArtistChart>
				.Create
				.ResponseAs<RawArtistChartResponse>();

			Assert.That(response2, Is.Not.EqualTo(response1));
		}

		private static void AssertIsCacheableResponse(Response response)
		{
			Assert.That(response, Is.Not.Null);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(response.Headers.ContainsKey("cache-control"), Is.True);
			Assert.That(response.Headers["cache-control"], Is.StringContaining("max-age="));
		}
	}
}
