using System.Xml.Linq;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Serialization
{
	[TestFixture]
	public class XmlErrorHandlerTests
	{
		[Test]
		public void Should_get_response_as_xml_with_valid_xml() {
			const string validXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"ok\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><serviceStatus><serverTime>2011-09-27T10:45:06Z</serverTime></serviceStatus></response>";
			var xmlErrorHandler = new XmlErrorHandler();

			XElement responseAsXml = xmlErrorHandler.GetResponseAsXml(validXml);
			Assert.That(responseAsXml.Name.LocalName, Is.EqualTo("response"));
			Assert.That(responseAsXml.Element("serviceStatus"), Is.Not.Null);
		}

		[Test]
		public void Should_get_response_as_xml_with_valid_xml_without_response() {
			const string validXml = "<xml>Hello world</xml>";
			var xmlErrorHandler = new XmlErrorHandler();

			XElement responseAsXml = xmlErrorHandler.GetResponseAsXml(validXml);
			Assert.That(responseAsXml.Name.LocalName, Is.EqualTo("xml"));
		}

		[Test]
		public void Should_get_response_as_xml_with_invalid_xml() {
			const string validXml = "ERROR--ERROR!";
			var xmlErrorHandler = new XmlErrorHandler();

			XElement responseAsXml = xmlErrorHandler.GetResponseAsXml(validXml);

			Assert.That(responseAsXml.Name.LocalName, Is.EqualTo("response"));
		}
	}
}