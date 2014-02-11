using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Responses.Parsing;
using SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Responses.Parsing
{
	[TestFixture]
	public class StringDeserializerTests
	{
		const string TestObjectXmlResponse = "<?xml version=\"1.0\"?><response xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" status=\"ok\" version=\"1.2\"><testObject id=\"1\"> <name>A big test object</name><listOfThings><string>one</string><string>two</string><string>three</string></listOfThings><listOfInnerObjects><InnerObject id=\"1\"><Name>Trevor</Name></InnerObject><InnerObject id=\"2\"><Name>Bill</Name></InnerObject></listOfInnerObjects></testObject></response>";
		const string EmptyXmlResponse = "<?xml version=\"1.0\"?><response xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" status=\"ok\" version=\"1.2\"></response>";


		[Test]
		public void should_deserialize_well_formed_xml()
		{
			var deserializer = new StringDeserializer<TestObject>();

			var testObject = deserializer.Deserialize(TestObjectXmlResponse);

			Assert.That(testObject, Is.Not.Null);
			Assert.That(testObject.Id, Is.EqualTo(1));
			Assert.That(testObject.Name, Is.EqualTo( "A big test object"));

			Assert.That(testObject.StringList, Is.Not.Null);
			Assert.That(testObject.StringList.Count, Is.GreaterThan(0));
			
			Assert.That(testObject.ObjectList, Is.Not.Null);
			Assert.That(testObject.ObjectList.Count, Is.GreaterThan(0));
		}

		[Test]
		public void should_deserialize_Empty_xml_to_empty_object()
		{
			var deserializer = new StringDeserializer<TestEmptyObject>();

			var testObject = deserializer.Deserialize(EmptyXmlResponse);

			Assert.That(testObject, Is.Not.Null);
		}

		[Test]
		public void should_throw_exception_when_deserialize_into_wrong_type()
		{
			var deserializer = new StringDeserializer<Status>();

			Assert.Throws<UnexpectedXmlContentException>(() => deserializer.Deserialize(TestObjectXmlResponse));
		}

		[Test]
		public void should_throw_exception_when_deserialize_into_wrong_type_such_as_one_that_is_not_wrapped_in_a_response_tag()
		{
			var deserializer = new StringDeserializer<Status>();

			Assert.Throws<UnexpectedXmlContentException>(() => deserializer.Deserialize(TestObjectXmlResponse.Replace("response", "rexponse")));
		}
	}
}
