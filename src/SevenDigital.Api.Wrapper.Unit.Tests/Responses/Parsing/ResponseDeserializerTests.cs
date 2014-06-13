using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;
using SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Responses.Parsing
{
	[TestFixture]
	public class ResponseDeserializerTests
	{
		const string XmlResponseText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><serviceStatus><serverTime>2014-06-07T19:00:00</serverTime></serviceStatus>";
		const string XmlWrongDtoResponseText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><shmerviceStatus><serverTime>2014-06-07T19:00:00</serverTime></shmerviceStatus>";
		const string JsonResponseText = "{ \"serviceStatus\" : { \"serverTime\": \"2014-06-07T19:00:00\" } }";

		const string TestObjectXmlResponse = "<?xml version=\"1.0\"?><response xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" status=\"ok\" version=\"1.2\"><testObject id=\"1\"> <name>A big test object</name><listOfThings><string>one</string><string>two</string><string>three</string></listOfThings><listOfInnerObjects><InnerObject id=\"1\"><Name>Trevor</Name></InnerObject><InnerObject id=\"2\"><Name>Bill</Name></InnerObject></listOfInnerObjects></testObject></response>";
		const string EmptyXmlResponse = "<?xml version=\"1.0\"?><response xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" status=\"ok\" version=\"1.2\"></response>";

		[Test]
		public void Should_parse_xml()
		{
			var response = new Response(HttpStatusCode.OK, XmlContentType(), XmlResponseText);

			var deserializer = new ResponseDeserializer();
			var status = deserializer.DeserializeResponse<Status>(response, false);

			Assert.That(status, Is.Not.Null);
		}

		[Test]
		public void Should_not_parse_wrong_xml()
		{
			var response = new Response(HttpStatusCode.OK, XmlContentType(), XmlWrongDtoResponseText);

			var deserializer = new ResponseDeserializer();
			Assert.Throws<UnexpectedXmlContentException>(() => deserializer.DeserializeResponse<Status>(response, false));
		}

		[Test]
		public void Should_parse_json()
		{
			var response = new Response(HttpStatusCode.OK, JsonContentType(), JsonResponseText);

			var deserializer = new ResponseDeserializer();
			var status = deserializer.DeserializeResponse<Status>(response, false);

			Assert.That(status, Is.Not.Null);
		}

		[Test]
		public void Should_not_parse_xml_identified_as_json()
		{
			var response = new Response(HttpStatusCode.OK, JsonContentType(), XmlResponseText);

			var deserializer = new ResponseDeserializer();

			Assert.Throws<JsonParseFailedException>(() => deserializer.DeserializeResponse<Status>(response, false));
		}

		[Test]
		public void Should_not_parse_json_identified_as_xml()
		{
			var response = new Response(HttpStatusCode.OK, XmlContentType(), JsonResponseText);

			var deserializer = new ResponseDeserializer();

			Assert.Throws<NonXmlContentException>(() => deserializer.DeserializeResponse<Status>(response, false));
		}

		[Test]
		public void should_deserialize_well_formed_xml()
		{
			var deserializer = new ResponseDeserializer();
			var response = new Response(HttpStatusCode.OK, XmlContentType(), TestObjectXmlResponse);

			var testObject = deserializer.DeserializeResponse<TestObject>(response, true);

			Assert.That(testObject, Is.Not.Null);
			Assert.That(testObject.Id, Is.EqualTo(1));
			Assert.That(testObject.Name, Is.EqualTo("A big test object"));

			Assert.That(testObject.StringList, Is.Not.Null);
			Assert.That(testObject.StringList.Count, Is.GreaterThan(0));

			Assert.That(testObject.ObjectList, Is.Not.Null);
			Assert.That(testObject.ObjectList.Count, Is.GreaterThan(0));
		}

		[Test]
		public void should_deserialize_Empty_xml_to_empty_object()
		{
			var deserializer = new ResponseDeserializer();
			var response = new Response(HttpStatusCode.OK, XmlContentType(), EmptyXmlResponse);

			var testObject = deserializer.DeserializeResponse<TestEmptyObject>(response, true);

			Assert.That(testObject, Is.Not.Null);
		}

		[Test]
		public void should_throw_exception_when_deserialize_into_wrong_type()
		{
			var deserializer = new ResponseDeserializer();
			var response = new Response(HttpStatusCode.OK, XmlContentType(), TestObjectXmlResponse);

			Assert.Throws<UnexpectedXmlContentException>(() => deserializer.DeserializeResponse<Status>(response, true));
		}

		[Test]
		public void should_throw_exception_when_deserialize_into_wrong_type_such_as_one_that_is_not_wrapped_in_a_response_tag()
		{
			var deserializer = new ResponseDeserializer();
			var wrongTag = TestObjectXmlResponse.Replace("response", "rexponse");
			var response = new Response(HttpStatusCode.OK, XmlContentType(), wrongTag);

			Assert.Throws<UnexpectedXmlContentException>(() => deserializer.DeserializeResponse<Status>(response, true));
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
