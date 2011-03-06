using System;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Repository;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointTests
{
	[TestFixture]
	[Category("Integration")]
	public class StatusTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver()); // TODO: Set up using castle?

			Status status = new FluentApi<Status>(httpGetResolver)
								.Resolve();

			Assert.That(status, Is.Not.Null);
			Assert.That(status.ServerTime.Day, Is.EqualTo(DateTime.Now.Day));
		}
	}
}
