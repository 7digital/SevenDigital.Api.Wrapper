using System;
using NUnit.Framework;
using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests
{
	[TestFixture]
	public class StatusTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			Status status = Api<Status>.Create.MakeRequest().Please();

			Assert.That(status, Is.Not.Null);
			Assert.That(status.ServerTime.Day, Is.EqualTo(DateTime.Now.Day));
		}
	}
}
