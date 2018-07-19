using System.Collections.Generic;
using System.Net.Http;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests
{
	[TestFixture]
	public class RouteParamsSubstitutorTests
	{
		private IApiUri _apiUri;

		[SetUp]
		public void Setup()
		{
			_apiUri = A.Fake<IApiUri>();
			A.CallTo(() => _apiUri.Uri).Returns("http://example.com");
			A.CallTo(() => _apiUri.SecureUri).Returns("https://example.com");
		}

		[Test]
		public void Should_return_data()
		{
			var requestData = new RequestData
				{
					HttpMethod = HttpMethod.Get,
					Endpoint = "testpath",
				};

			var routeParamsSubstitutor = new RouteParamsSubstitutor(_apiUri);
			var result = routeParamsSubstitutor.SubstituteParamsInRequest(requestData);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.AbsoluteUrl, Is.Not.Empty);
			Assert.That(result.Parameters, Is.Not.Null);
		}

		[Test]
		public void Should_substitue_route_parameters()
		{
			var requestData = new RequestData
			{
				Endpoint = "something/{route}",
				Parameters = new Dictionary<string, string>
					{
						{"route","routevalue"}
					}
			};

			var routeParamsSubstitutor = new RouteParamsSubstitutor(_apiUri);
			var result = routeParamsSubstitutor.SubstituteParamsInRequest(requestData);

			Assert.That(result.AbsoluteUrl, Is.StringContaining("something/routevalue"));
		}

		[Test]
		public void Should_substitue_multiple_route_parameters()
		{
			var requestData = new RequestData
			{
				Endpoint = "something/{firstRoute}/{secondRoute}/thrid/{thirdRoute}",
				Parameters = new Dictionary<string, string>
					{
						{"firstRoute" , "firstValue"},
						{"secondRoute","secondValue"},
						{"thirdRoute" , "thirdValue"}
							
					}
			};

			var routeParamsSubstitutor = new RouteParamsSubstitutor(_apiUri);
			var result = routeParamsSubstitutor.SubstituteParamsInRequest(requestData);

			Assert.That(result.AbsoluteUrl, Is.StringContaining("something/firstvalue/secondvalue/thrid/thirdvalue"));
		}

		[Test]
		public void Routes_should_be_case_insensitive()
		{
			var requestData = new RequestData
			{
				Endpoint = "something/{firstRoUte}/{secOndrouTe}/thrid/{tHirdRoute}",
				Parameters = new Dictionary<string, string>
					{
						{"firstRoute" , "firstValue"},
						{"secondRoute","secondValue"},
						{"thirdRoute" , "thirdValue"}
							
					}
			};

			var routeParamsSubstitutor = new RouteParamsSubstitutor(_apiUri);
			var result = routeParamsSubstitutor.SubstituteParamsInRequest(requestData);

			Assert.That(result.AbsoluteUrl, Is.StringContaining("something/firstvalue/secondvalue/thrid/thirdvalue"));
		}

		[Test]
		public void Should_handle_dashes_and_numbers()
		{
			var requestData = new RequestData
			{
				Endpoint = "something/{route-66}",
				Parameters = new Dictionary<string, string>
					{
						{"route-66","routevalue"}
					}
			};

			var routeParamsSubstitutor = new RouteParamsSubstitutor(_apiUri);
			var result = routeParamsSubstitutor.SubstituteParamsInRequest(requestData);

			Assert.That(result.AbsoluteUrl, Is.StringContaining("something/routevalue"));
		}

		[Test]
		public void Should_remove_parameters_that_match()
		{
			var requestData = new RequestData
			{
				Endpoint = "something/{route}",
				Parameters = new Dictionary<string, string>
					{
						{"route","routevalue"},
						{"foo","bar"}
					}
			};

			var routeParamsSubstitutor = new RouteParamsSubstitutor(_apiUri);
			var result = routeParamsSubstitutor.SubstituteParamsInRequest(requestData);

			Assert.That(result.Parameters.ContainsKey("route"), Is.False);
			Assert.That(result.Parameters.ContainsKey("foo"), Is.True);
		}

		[Test]
		public void Should_remove_multiple_parameters_that_match()
		{
			var requestData = new RequestData
			{
				Endpoint = "something/{firstRoute}/{secondRoute}/thrid/{thirdRoute}",
				Parameters = new Dictionary<string, string>
					{
						{"firstRoute" , "firstValue"},
						{"secondRoute","secondValue"},
						{"thirdRoute" , "thirdValue"},
						{"foo","bar"},
					}
			};

			var routeParamsSubstitutor = new RouteParamsSubstitutor(_apiUri);
			var result = routeParamsSubstitutor.SubstituteParamsInRequest(requestData);

			Assert.That(result.Parameters.ContainsKey("firstRoute"), Is.False);
			Assert.That(result.Parameters.ContainsKey("secondRoute"), Is.False);
			Assert.That(result.Parameters.ContainsKey("thirdRoute"), Is.False);
			Assert.That(result.Parameters.ContainsKey("foo"), Is.True);
		}
	}
}