using System;
using System.Linq.Expressions;
using System.Xml.Serialization;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Schema;
using System.Threading;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class FluentApiTests
	{
		private const string VALID_STATUS_XML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"ok\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\"><serviceStatus><serverTime>2011-05-31T15:31:22Z</serverTime></serviceStatus></response>";

		[Test]
		public void Should_fire_endpointresolver_with_correct_endpoint_on_resolve()
		{
			var endpointResolver = A.Fake<IEndpointResolver>();
			A.CallTo(() => endpointResolver.HitEndpoint(A<EndPointInfo>.Ignored)).Returns(VALID_STATUS_XML);

			new FluentApi<Status>(endpointResolver).Please();

			Expression<Func<string>> callWithEndpointStatus =
				() => endpointResolver.HitEndpoint(A<EndPointInfo>.That.Matches(x => x.Uri == "status"));

			A.CallTo(callWithEndpointStatus).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_endpointresolver_with_correct_methodname_on_resolve()
		{
			var endpointResolver = A.Fake<IEndpointResolver>();
			A.CallTo(() => endpointResolver.HitEndpoint(A<EndPointInfo>.Ignored)).Returns(VALID_STATUS_XML);

			new FluentApi<Status>(endpointResolver).WithMethod("POST").Please();

			Expression<Func<string>> callWithMethodPost =
				() => endpointResolver.HitEndpoint(A<EndPointInfo>.That.Matches(x => x.HttpMethod == "POST"));

			A.CallTo(callWithMethodPost).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_endpointresolver_with_correct_parameters_on_resolve()
		{
			var endpointResolver = A.Fake<IEndpointResolver>();
			A.CallTo(() => endpointResolver.HitEndpoint(A<EndPointInfo>.Ignored)).Returns(VALID_STATUS_XML);

			new FluentApi<Status>(endpointResolver).WithParameter("artistId", "123").Please();

			Expression<Func<string>> callWithArtistId123 =
				() => endpointResolver.HitEndpoint(A<EndPointInfo>.That.Matches(x => x.Parameters["artistId"] == "123"));

			A.CallTo(callWithArtistId123).MustHaveHappened();

		}

		[Test]
		public void should_put_payload_in_action_result()
		{
			var endpointResolver = new FakeEndpointResolver { StubPayload = VALID_STATUS_XML };
			var reset = new AutoResetEvent(false);

			new FluentApi<Status>(endpointResolver)
				.PleaseAsync(
				status =>
				{
					Assert.That(status, Is.Not.Null);
					reset.Set();
				});

			reset.WaitOne(1000 * 60);
			Assert.True(true);
		}

		public class FakeEndpointResolver : IEndpointResolver
		{
			public string HitEndpoint(EndPointInfo endPointInfo)
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

			public string StubPayload { get; set; }
		}
	}

}
