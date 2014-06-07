using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Responses
{
	[TestFixture]
	public class ResponseDeserializerTests
	{
		const string XmlResponseText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><serviceStatus><serverTime>2014-06-07T19:00:00</serverTime></serviceStatus>";
		const string JsonResponseText = "{ \"serviceStatus\" : { \"serverTime\": \"2014-06-07T19:00:00\" } }";

		[Test]
		public void Should_parse_xml()
		{
			var response = new Response(HttpStatusCode.OK, XmlContentType(), XmlResponseText);

			var deserializer = new ResponseDeserializer();
			var status = deserializer.ResponseAs<Status>(response);

			Assert.That(status, Is.Not.Null);
		}

		[Test]
		public void Should_parse_json()
		{
			var response = new Response(HttpStatusCode.OK, JsonContentType(), JsonResponseText);

			var deserializer = new ResponseDeserializer();
			var status = deserializer.ResponseAs<Status>(response);

			Assert.That(status, Is.Not.Null);
		}

		private IDictionary<string, string> XmlContentType()
		{
			return new Dictionary<string, string>
				{
					{"Content-Type", "application/xml"}
				};
		}

		private IDictionary<string, string> JsonContentType()
		{
			return new Dictionary<string, string>
				{
					{"Content-Type", "application/json"}
				};
		}
	}
}
