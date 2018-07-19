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
		public async void Can_see_custom_traceId()
		{
			var customTraceId = Guid.NewGuid().ToString();
			var status = await Api<Status>.Create.WithTraceId(customTraceId).Response();

			Assert.That(status.OriginalRequest.TraceId, Is.EqualTo(customTraceId));
		}

		[Test]
		public async void Can_specify_2_different_trace_ids_on_different_requests()
		{
			var customTraceId1 = Guid.NewGuid().ToString();
			var customTraceId2 = Guid.NewGuid().ToString();

			var fluentApi = Api<Status>.Create;
			var status = await fluentApi.WithTraceId(customTraceId1).Response();
			var status2 = await fluentApi.WithTraceId(customTraceId2).Response();

			Assert.That(status.OriginalRequest.TraceId, Is.EqualTo(customTraceId1));
			Assert.That(status2.OriginalRequest.TraceId, Is.EqualTo(customTraceId2));
		}
	}
}
