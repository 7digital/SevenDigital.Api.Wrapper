using System.Xml.Linq;
using NUnit.Framework;

namespace SevenDigital.Api.Dynamic.Unit.Tests
{
	[TestFixture]
	public class DynamicXmlParserTests
	{
		[Test]
		public void Can_deal_with_xml() {
			string xml = "<books><book id='1'><name>Book with 2 authors</name><authors><author id='1'>Billy</author></authors></book><book id='2'><name>The Bobbit</name></book></books>";
			var xDocument = XDocument.Parse(xml);
			
			dynamic dx = new DynamicXmlParser(xDocument);
			var condition = dx.book[1].name[0].value;
			
			Assert.That(condition, Is.EqualTo("The Bobbit"));
		}
	}
}