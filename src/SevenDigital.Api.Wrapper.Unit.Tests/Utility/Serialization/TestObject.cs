using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Serialization
{
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

	[Serializable]
	public class TestEmptyObject
	{
	}
}