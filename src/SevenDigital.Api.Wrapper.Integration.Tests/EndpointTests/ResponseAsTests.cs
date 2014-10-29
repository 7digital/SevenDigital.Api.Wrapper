using System;
using System.Net;
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
	public class ResponseAsTests
	{
		[Test]
		public async void TestStatusResponse()
		{
			var statusResponse = await Api<Status>.Create.Response();

			Assert.That(statusResponse, Is.Not.Null);
			Assert.That(statusResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
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
