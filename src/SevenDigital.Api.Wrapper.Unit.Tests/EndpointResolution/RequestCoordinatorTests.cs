using System.Collections.Generic;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution
{
	[TestFixture]
	public class RequestCoordinatorTests
	{
		private RequestCoordinator _requestCoordinator;

		[SetUp]
		public void Setup()
		{
			var httpClient = A.Fake<IHttpClient>();
			A.CallTo(() => httpClient.Get(A<GetRequest>.Ignored)).Returns(new Response(HttpStatusCode.OK, "OK"));

			var apiUri = A.Fake<IApiUri>();
			A.CallTo(() => apiUri.Uri)
				.Returns("http://uri/");
			var allRequestHandlers = RequestHandlerFactory.AllRequestHandlers(new AppSettingsCredentials(), apiUri);
			//var allRequestHandlers = new List<RequestHandler>(){A.Fake<RequestHandler>()};
			_requestCoordinator = new RequestCoordinator(httpClient, allRequestHandlers);
		}

		[Test]
		public void should_substitue_route_parameters()
		{
			var requestData = new RequestData
			{
				UriPath = "something/{route}",
				Parameters = new Dictionary<string, string>
					{
						{"route","routevalue"}
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(requestData);

			Assert.That(result,Is.StringContaining("something/routevalue"));
		}

		[Test]
		public void should_substitue_multiple_route_parameters()
		{
			var requestData = new RequestData
			{
				UriPath = "something/{firstRoute}/{secondRoute}/thrid/{thirdRoute}",
				Parameters = new Dictionary<string, string>
					{
						{"firstRoute" , "firstValue"},
						{"secondRoute","secondValue"},
						{"thirdRoute" , "thirdValue"}
							
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(requestData);

			Assert.That(result, Is.StringContaining("something/firstvalue/secondvalue/thrid/thirdvalue"));
		}

		[Test]
		public void routes_should_be_case_insensitive()
		{
			var requestData = new RequestData
			{
				UriPath = "something/{firstRoUte}/{secOndrouTe}/thrid/{tHirdRoute}",
				Parameters = new Dictionary<string, string>
					{
						{"firstRoute" , "firstValue"},
						{"secondRoute","secondValue"},
						{"thirdRoute" , "thirdValue"}
							
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(requestData);

			Assert.That(result, Is.StringContaining("something/firstvalue/secondvalue/thrid/thirdvalue"));
		}

		[Test]
		public void should_handle_dashes_and_numbers()
		{
			var requestData = new RequestData
			{
				UriPath = "something/{route-66}",
				Parameters = new Dictionary<string, string>
					{
						{"route-66","routevalue"}
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(requestData);

			Assert.That(result, Is.StringContaining("something/routevalue"));
		}

		[Test]
		public void should_remove_parameters_that_match()
		{
			var requestData = new RequestData
			{
				UriPath = "something/{route-66}",
				Parameters = new Dictionary<string, string>
					{
						{"route-66","routevalue"}
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(requestData);

			Assert.That(result, Is.Not.StringContaining("route-66=routevalue"));
		}

	}
}
