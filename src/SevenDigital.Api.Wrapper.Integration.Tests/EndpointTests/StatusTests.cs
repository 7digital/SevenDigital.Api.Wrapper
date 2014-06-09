using System;
using System.Xml.Serialization;

using NUnit.Framework;
using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests
{
	[XmlRoot("response")]
	public class RawStatusResponse
	{
		[XmlElement("serviceStatus")]
		public Status ServiceStatus { get; set; }
	}

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
		public async void TestStatusResponseAs()
		{
			var statusResponse = await Api<Status>.Create.ResponseAs<RawStatusResponse>();

			Assert.That(statusResponse, Is.Not.Null);
			Assert.That(statusResponse.ServiceStatus, Is.Not.Null);
			Assert.That(statusResponse.ServiceStatus.ServerTime.Day, Is.EqualTo(DateTime.Now.Day));
		}
	}
}
