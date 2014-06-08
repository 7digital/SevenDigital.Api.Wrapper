using System.Collections.Generic;
using System.Net;

using NUnit.Framework;

using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Responses
{
	[TestFixture]
	public class ResponseDeserializerTests
	{
		const string XmlResponseText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><serviceStatus><serverTime>2014-06-07T19:00:00</serverTime></serviceStatus>";
		const string XmlWrongDtoResponseText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><shmerviceStatus><serverTime>2014-06-07T19:00:00</serverTime></shmerviceStatus>";
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
		public void Should_not_parse_wrong_xml()
		{
			var response = new Response(HttpStatusCode.OK, XmlContentType(), XmlWrongDtoResponseText);

			var deserializer = new ResponseDeserializer();
			Assert.Throws<UnexpectedXmlContentException>(() => deserializer.ResponseAs<Status>(response));
		}

		[Test]
		public void Should_parse_json()
		{
			var response = new Response(HttpStatusCode.OK, JsonContentType(), JsonResponseText);

			var deserializer = new ResponseDeserializer();
			var status = deserializer.ResponseAs<Status>(response);

			Assert.That(status, Is.Not.Null);
		}

		[Test]
		public void Should_not_parse_xml_identified_as_json()
		{
			var response = new Response(HttpStatusCode.OK, JsonContentType(), XmlResponseText);

			var deserializer = new ResponseDeserializer();

			Assert.Throws<JsonParseFailedException>(() => deserializer.ResponseAs<Status>(response));
		}

		[Test]
		public void Should_not_parse_json_identified_as_xml()
		{
			var response = new Response(HttpStatusCode.OK, XmlContentType(), JsonResponseText);

			var deserializer = new ResponseDeserializer();

			Assert.Throws<NonXmlContentException>(() => deserializer.ResponseAs<Status>(response));
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
