﻿using System.Collections.Generic;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution
{
	[TestFixture]
	public class EndpointResolverTests
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

			_requestCoordinator = new RequestCoordinator(httpClient, new UrlSigner(), new AppSettingsCredentials(), apiUri);
		}

		[Test]
		public void should_substitue_route_parameters()
		{
			var endpointContext = new EndpointContext
			{
				UriPath = "something/{route}",
			};
			var requestData = new RequestContext
			{
				Parameters = new Dictionary<string, string>
					{
						{"route","routevalue"}
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(endpointContext, requestData);

			Assert.That(result,Is.StringContaining("something/routevalue"));
		}

		[Test]
		public void should_substitue_multiple_route_parameters()
		{
			var endpointContext = new EndpointContext
			{
				UriPath = "something/{firstRoute}/{secondRoute}/thrid/{thirdRoute}",
			};

			var requestData = new RequestContext
			{
				Parameters = new Dictionary<string, string>
					{
						{"firstRoute" , "firstValue"},
						{"secondRoute","secondValue"},
						{"thirdRoute" , "thirdValue"}
							
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(endpointContext, requestData);

			Assert.That(result, Is.StringContaining("something/firstvalue/secondvalue/thrid/thirdvalue"));
		}

		[Test]
		public void routes_should_be_case_insensitive()
		{
			var endpointContext = new EndpointContext
			{
				UriPath = "something/{firstRoUte}/{secOndrouTe}/thrid/{tHirdRoute}",
			};
			var requestData = new RequestContext
			{
				Parameters = new Dictionary<string, string>
					{
						{"firstRoute" , "firstValue"},
						{"secondRoute","secondValue"},
						{"thirdRoute" , "thirdValue"}
							
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(endpointContext, requestData);

			Assert.That(result, Is.StringContaining("something/firstvalue/secondvalue/thrid/thirdvalue"));
		}

		[Test]
		public void should_handle_dashes_and_numbers()
		{
			var endpointContext = new EndpointContext
			{
				UriPath = "something/{route-66}",
			};
			var requestData = new RequestContext
			{
				Parameters = new Dictionary<string, string>
					{
						{"route-66","routevalue"}
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(endpointContext, requestData);

			Assert.That(result, Is.StringContaining("something/routevalue"));
		}

		[Test]
		public void should_remove_parameters_that_match()
		{
			var endpointContext = new EndpointContext
			{
				UriPath = "something/{route-66}",
			};
			var requestData = new RequestContext
			{
				Parameters = new Dictionary<string, string>
					{
						{"route-66","routevalue"}
					}
			};

			var result = _requestCoordinator.ConstructEndpoint(endpointContext, requestData);

			Assert.That(result, Is.Not.StringContaining("route-66=routevalue"));
		}

	}
}
