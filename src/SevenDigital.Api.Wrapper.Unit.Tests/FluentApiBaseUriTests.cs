using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;

using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class FluentApiBaseUriTests
	{
		private const string VALID_STATUS_XML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"ok\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\"><serviceStatus><serverTime>2011-05-31T15:31:22Z</serverTime></serviceStatus></response>";

		private readonly Response _stubResponse = new Response(HttpStatusCode.OK, VALID_STATUS_XML);

		private IHttpClient StubHttpClient()
		{
			var httpClient = A.Fake<IHttpClient>();
			A.CallTo(() => httpClient.Send(A<Request>.Ignored)).Returns(Task.FromResult(_stubResponse));
			return httpClient;
		}

		[Test]
		public async void Should_set_base_uri_provider_if_it_is_present_on_incoming_dto()
		{
			var requestBuilder = A.Fake<IRequestBuilder>();
			var httpClient = StubHttpClient();
			var responseParser = A.Fake<IResponseParser>();

			await new FluentApi<EndpointWithOwnBaseUri>(httpClient, requestBuilder, responseParser)
				.Please();

			Expression<Func<Request>> callWithExpectedPayload = () =>
				requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.BaseUriProvider.GetType() == typeof(EndpointWithOwnBaseUri)));

			A.CallTo(callWithExpectedPayload).MustHaveHappened();
		}
	}

	[ApiEndpoint("foo")]
	public class EndpointWithOwnBaseUri : IBaseUriProvider
	{
		public string BaseUri(RequestData requestData)
		{
			return "http://www.7dizzle.com";
		}
	}
}
