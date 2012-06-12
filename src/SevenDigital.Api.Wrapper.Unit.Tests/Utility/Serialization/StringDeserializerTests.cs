using System;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Serialization
{
	[TestFixture]
	public class StringDeserializerTests
	{
		const string xml = "<?xml version=\"1.0\"?><response xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><testObject id=\"1\"> <name>A big test object</name><listOfThings><string>one</string><string>two</string><string>three</string></listOfThings><listOfInnerObjects><InnerObject id=\"1\"><Name>Trevor</Name></InnerObject><InnerObject id=\"2\"><Name>Bill</Name></InnerObject></listOfInnerObjects></testObject></response>";

		[Test]
		public void should_deserialize_well_formed_xml()
		{
			var deserializer = new StringDeserializer<TestObject>();

			var testObject = deserializer.Deserialize(xml);

			Assert.That(testObject.Id, Is.EqualTo(1));
			Assert.That(testObject.Name, Is.EqualTo( "A big test object"));
		}

		[Test]
		public void should_throw_exception_when_deserialize_into_wrong_type()
		{
			var deserializer = new StringDeserializer<Status>();

			Assert.Throws<InvalidOperationException>(() => deserializer.Deserialize(xml));
		}
	}
}
