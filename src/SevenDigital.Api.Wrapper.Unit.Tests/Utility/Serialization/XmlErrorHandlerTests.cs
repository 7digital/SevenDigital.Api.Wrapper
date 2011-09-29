using System.Xml.Linq;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Serialization
{
	[TestFixture]
	public class XmlErrorHandlerTests
	{
		[Test]
		public void Should_get_response_as_xml_with_valid_api_xml() {
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

		[Test]
		public void Should_assert_error_with_valid_api_xml() {
			const string validXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"1001\"><errorMessage>Requires parameter q is missing</errorMessage></error></response>";
			var xmlErrorHandler = new XmlErrorHandler();
			XElement responseAsXml = xmlErrorHandler.GetResponseAsXml(validXml);
			Assert.Throws<ApiXmlException>(() => xmlErrorHandler.AssertError(responseAsXml));
		}

		[Test]
		public void Should_not_fail_if_xml_is_a_malformed_api_error() {
			const string validXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error><errorme></errorme></error></response>";
			var xmlErrorHandler = new XmlErrorHandler();
			XElement responseAsXml = xmlErrorHandler.GetResponseAsXml(validXml);
			Assert.Throws<ApiXmlException>(() => xmlErrorHandler.AssertError(responseAsXml));
		}

		[Test]
		public void Should_not_fail_if_xml_is_not_an_api_error() {
			const string validXml = "<xml>Hello world</xml>";
			var xmlErrorHandler = new XmlErrorHandler();
			XElement responseAsXml = xmlErrorHandler.GetResponseAsXml(validXml);
			Assert.DoesNotThrow(() => xmlErrorHandler.AssertError(responseAsXml));
		}
	}
}