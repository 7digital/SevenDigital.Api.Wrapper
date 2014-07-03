using System;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema.Baskets;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses.Parsing;
namespace SevenDigital.Api.Wrapper.Unit.Tests.Endpoints.Baskets
{
	[TestFixture]
	public class BasketEndpointTests
	{
		[Test]
		public void Should_not_remove_track_id_parameter_when_adding_a_release_to_basket()
		{
			var requestBuilder = A.Fake<IRequestBuilder>();
			var responseParser = A.Fake<IResponseParser>();
			var httpClient = A.Fake<IHttpClient>();

			var basketEndpoint = new FluentApi<AddItemToBasket>(httpClient, requestBuilder, responseParser);
			basketEndpoint.UseBasketId(NewBasketId()).ForReleaseId(1).ForTrackId(1);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"));

			basketEndpoint.UseBasketId(NewBasketId());
			Assert.That(basketEndpoint.Parameters.Keys.Contains("releaseId"), Is.True);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"), Is.True);
		}

		private string NewBasketId()
		{
			return Guid.NewGuid().ToString();
		}
	}
}