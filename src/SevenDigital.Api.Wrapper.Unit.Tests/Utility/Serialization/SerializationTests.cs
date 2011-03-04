using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Serialization
{
	[TestFixture]
	public class SerializationTests
	{
		[Test]
		public void Can_serialize_object()
		{
			var testObject = GetTestObject();
			var xmlSerializer = new XmlSerializer<TestObject>();
			Assert.DoesNotThrow(() =>xmlSerializer.Serialize(testObject));
			var xPathNavigable = xmlSerializer.Serialize(testObject) as XmlDocument;
			Assert.That(xPathNavigable, Is.Not.Null);
		}

		[Test]
		public void Can_deserialize_object()
		{
			const string xml = "<?xml version=\"1.0\"?><testObject xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" id=\"1\"><name>A big test object</name><listOfThings><string>one</string><string>two</string><string>three</string></listOfThings><listOfInnerObjects><InnerObject id=\"1\"><Name>Trevor</Name></InnerObject><InnerObject id=\"2\"><Name>Bill</Name></InnerObject></listOfInnerObjects></testObject>";
			var xmlSerializer = new XmlSerializer<TestObject>();
			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			Assert.DoesNotThrow(() => xmlSerializer.DeSerialize(xmlDocument));

			TestObject testObject = xmlSerializer.DeSerialize(xmlDocument);

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
