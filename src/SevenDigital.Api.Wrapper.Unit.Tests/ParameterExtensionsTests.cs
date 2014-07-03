using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema.Baskets;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class ParameterExtensionsTests
	{
		private IHttpClient _httpClient;
		private IRequestBuilder _requestBuilder;
		private IResponseParser _responseParser;

		[SetUp]
		public void SetUp()
		{
			_httpClient = A.Fake<IHttpClient>();
			_requestBuilder = A.Fake<IRequestBuilder>();
			_responseParser = A.Fake<IResponseParser>();
		}

		[Test]
		public void Should_add_int_parameter()
		{
			var basketEndpoint = new FluentApi<AddItemToBasket>(_httpClient, _requestBuilder, _responseParser);
			basketEndpoint.WithParameter("answer", 42);

			Assert.That(basketEndpoint.Parameters.Keys.Contains("answer"));
			Assert.That(basketEndpoint.Parameters["answer"], Is.EqualTo("42"));
		}

		[Test]
		public void Should_add_decimal_parameter()
		{
			var basketEndpoint = new FluentApi<AddItemToBasket>(_httpClient, _requestBuilder, _responseParser);
			basketEndpoint.WithParameter("answer", 42.12m);

			Assert.That(basketEndpoint.Parameters.Keys.Contains("answer"));
			Assert.That(basketEndpoint.Parameters["answer"], Is.EqualTo("42.12"));
		}

	}
}
