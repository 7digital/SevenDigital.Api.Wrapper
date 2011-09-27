using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Serialization
{
	[TestFixture]
	public class DeSerializationTests
	{
		[Test]
		public void Can_deserialize_object()
		{
            const string xml = "<?xml version=\"1.0\"?><response xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><testObject id=\"1\"> <name>A big test object</name><listOfThings><string>one</string><string>two</string><string>three</string></listOfThings><listOfInnerObjects><InnerObject id=\"1\"><Name>Trevor</Name></InnerObject><InnerObject id=\"2\"><Name>Bill</Name></InnerObject></listOfInnerObjects></testObject></response>";
		    var xmlSerializer = new ApiXmlDeSerializer<TestObject>(new ApiResourceDeSerializer<TestObject>(), new XmlErrorHandler());

            Assert.DoesNotThrow(() => xmlSerializer.DeSerialize(xml));

            TestObject testObject = xmlSerializer.DeSerialize(xml);

			Assert.That(testObject.Id, Is.EqualTo(GetTestObject().Id));
		}

		private static TestObject GetTestObject()
		{
			return new TestObject { Id = 1, Name = "A big test object", StringList = new List<string> { "one", "two", "three" }, ObjectList = new List<InnerObject> { new InnerObject { Id = 1, Name = "Trevor" }, new InnerObject { Id = 2, Name = "Bill" } } };
		}
	}

	[Serializable]
	[XmlRoot("testObject")]
	public class TestObject
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlArray("listOfThings")]
		public List<string> StringList { get; set; }

		[XmlArray("listOfInnerObjects")]
		public List<InnerObject> ObjectList { get; set; }

	}

	[Serializable]
	[XmlRoot("innerObject")]
	public class InnerObject
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("Name")]
		public string Name { get; set; }
	}

}
