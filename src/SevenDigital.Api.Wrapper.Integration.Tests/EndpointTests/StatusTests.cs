using System;

using NUnit.Framework;
using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests
{
	[TestFixture]
	public class StatusTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var status = await Api<Status>.Create.Please();

			Assert.That(status, Is.Not.Null);
			Assert.That(status.ServerTime.Day, Is.EqualTo(DateTime.Now.Day));
		}

		[Test]
		public async void Can_see_default_traceId_as_guid_parseable_string()
		{
			var status = await Api<Status>.Create.Response();

			var traceId = status.OriginalRequest.TraceId;
			Assert.That(traceId, Is.Not.Null);
			Assert.DoesNotThrow(() => Guid.Parse(traceId));
		}

		[Test]
		public async void Two_separate_requests_get_different_traceIds()
		{
			var fluentApi = Api<Status>.Create;
			var status = await fluentApi.Response();
			var status2 = await fluentApi.Response();

			var traceId = status.OriginalRequest.TraceId;
			var traceId2 = status2.OriginalRequest.TraceId;
			Assert.That(status.OriginalRequest.TraceId, Is.Not.Null);
			Assert.DoesNotThrow(() => Guid.Parse(traceId));

			Assert.That(status.OriginalRequest.TraceId, Is.Not.Null);
			Assert.DoesNotThrow(() => Guid.Parse(traceId2));

			Assert.That(traceId, Is.Not.EqualTo(traceId2));
		}

		[Test]
		public async void Can_see_custom_traceId()
		{
			var customTraceId = Guid.NewGuid().ToString();
			var status = await Api<Status>.Create.WithTraceId(customTraceId).Response();

			Assert.That(status.OriginalRequest.TraceId, Is.EqualTo(customTraceId));
		}
	}
}
