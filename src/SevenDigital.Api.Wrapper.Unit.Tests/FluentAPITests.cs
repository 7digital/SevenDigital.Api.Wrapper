using System;
using System.Linq.Expressions;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Schema;
using System.Threading;
using SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class FluentApiTests
	{
		private const string VALID_STATUS_XML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"ok\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\"><serviceStatus><serverTime>2011-05-31T15:31:22Z</serverTime></serviceStatus></response>";

		[Test]
		public void Should_fire_requestcoordinator_with_correct_endpoint_on_resolve()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<EndPointInfo>.Ignored)).Returns(VALID_STATUS_XML);

			new FluentApi<Status>(requestCoordinator).Please();

			Expression<Func<string>> callWithEndpointStatus =
				() => requestCoordinator.HitEndpoint(A<EndPointInfo>.That.Matches(x => x.Uri == "status"));

			A.CallTo(callWithEndpointStatus).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_requestcoordinator_with_correct_methodname_on_resolve()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<EndPointInfo>.Ignored)).Returns(VALID_STATUS_XML);

			new FluentApi<Status>(requestCoordinator).WithMethod("POST").Please();

			Expression<Func<string>> callWithMethodPost =
				() => requestCoordinator.HitEndpoint(A<EndPointInfo>.That.Matches(x => x.HttpMethod == "POST"));

			A.CallTo(callWithMethodPost).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_requestcoordinator_with_correct_parameters_on_resolve()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<EndPointInfo>.Ignored)).Returns(VALID_STATUS_XML);

			new FluentApi<Status>(requestCoordinator).WithParameter("artistId", "123").Please();

			Expression<Func<string>> callWithArtistId123 =
				() => requestCoordinator.HitEndpoint(A<EndPointInfo>.That.Matches(x => x.Parameters["artistId"] == "123"));

			A.CallTo(callWithArtistId123).MustHaveHappened();

		}
		[Test]
		public void Should_use_custom_http_client()
		{
			var fakeRequestCoordinator = A.Fake<IRequestCoordinator>();
			var fakeHttpClient = new FakeHttpClient();

			new FluentApi<Status>(fakeRequestCoordinator).UsingClient(fakeHttpClient);

			Assert.That(fakeRequestCoordinator.HttpClient, Is.EqualTo(fakeHttpClient));
		}

		[Test]
		public void should_put_payload_in_action_result()
		{
			var requestCoordinator = new FakeRequestCoordinator { StubPayload = VALID_STATUS_XML };
			var reset = new AutoResetEvent(false);

			new FluentApi<Status>(requestCoordinator)
				.PleaseAsync(
				status =>
				{
					Assert.That(status, Is.Not.Null);
					reset.Set();
				});

			reset.WaitOne(1000 * 60);
			Assert.True(true);
		}

		

		public class FakeRequestCoordinator : IRequestCoordinator
		{
			public string HitEndpoint(EndPointInfo endPointInfo)
			{
				throw new NotImplementedException();
			}

			public IResponse HitEndpointAndGetResponse(EndPointInfo endPointInfo)
			{
				throw new NotImplementedException();
			}

			public void HitEndpointAsync(EndPointInfo endPointInfo, Action<string> payload)
			{
				payload(StubPayload);
			}

			public string ConstructEndpoint(EndPointInfo endPointInfo)
			{
				throw new NotImplementedException();
			}

			public IHttpClient HttpClient
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public string StubPayload { get; set; }
		}
	}

}
