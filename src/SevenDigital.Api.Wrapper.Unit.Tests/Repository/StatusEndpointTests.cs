using System;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Repository;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Repository
{
	[TestFixture]
	[Category("Integration")]
	public class StatusEndpointTests
	{
		[Test]
		public void Can_deserialize_error()
		{
			
		}

		[Test]
		public void Can_deserialize_status()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());
			var statusEndpoint = new StatusEndpoint(httpGetResolver);
			Status status = statusEndpoint.Get();
			Assert.That(status, Is.Not.Null);
			Assert.That(status.ServerTime.Day, Is.EqualTo(DateTime.Now.Day));
		}
	}
}
