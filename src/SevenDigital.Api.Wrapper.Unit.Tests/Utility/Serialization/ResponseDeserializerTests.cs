using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Serialization
{
	[TestFixture]
	public class ResponseDeserializerTests
	{
		//well formed response
		[Test]
		public void Can_deserialize_object()
		{
			const string xml = "<?xml version=\"1.0\"?><response xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><testObject id=\"1\"> <name>A big test object</name><listOfThings><string>one</string><string>two</string><string>three</string></listOfThings><listOfInnerObjects><InnerObject id=\"1\"><Name>Trevor</Name></InnerObject><InnerObject id=\"2\"><Name>Bill</Name></InnerObject></listOfInnerObjects></testObject></response>";

			var stubResponse = new Response
				{
					StatusCode = HttpStatusCode.OK,
					Body = xml
				};

			var xmlSerializer = new ResponseDeserializer<TestObject>();

			Assert.DoesNotThrow(() => xmlSerializer.Deserialize(stubResponse));

			TestObject testObject = xmlSerializer.Deserialize(stubResponse);

			Assert.That(testObject.Id, Is.EqualTo(1));
		}

		[Test]
		public void Can_deserialize_server_error()
		{
			const string errorXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"1001\"><errorMessage>Test error</errorMessage></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.InternalServerError,
				Body = errorXml
			};

			var xmlSerializer = new ResponseDeserializer<TestObject>();

			var ex = Assert.Throws<ApiXmlException>(() => xmlSerializer.Deserialize(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.Message, Is.StringStarting("Error response"));
			Assert.That(ex.Message, Is.StringEnding(errorXml));

			Assert.That(ex.Error.ErrorMessage, Is.EqualTo("Test error"));
			Assert.That(ex.Error.Code, Is.EqualTo(1001));
		}

		[Test]
		public void Can_deserialize_well_formed_error()
		{
			const string errorXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"1001\"><errorMessage>Test error</errorMessage></error></response>";
			var response = new Response
				{
					StatusCode = HttpStatusCode.OK,
					Body = errorXml
				};

			var xmlSerializer = new ResponseDeserializer<TestObject>();

			var ex = Assert.Throws<ApiXmlException>(() => xmlSerializer.Deserialize(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.Message, Is.StringStarting("Error response"));
			Assert.That(ex.Message, Is.StringEnding(errorXml));
			Assert.That(ex.Error.Code, Is.EqualTo(1001));
			Assert.That(ex.Error.ErrorMessage, Is.EqualTo("Test error"));
		}

		//badly formed xmls / response
		[Test]
		public void Should_not_fail_if_xml_is_a_malformed_api_error()
		{
			const string badError = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error><errorme></errorme></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = badError
			};

			var xmlSerializer = new ResponseDeserializer<TestObject>();

			var ex = Assert.Throws<ApiXmlException>(() => xmlSerializer.Deserialize(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.Error.ErrorMessage, Is.StringStarting("XML error parse failed"));
		}

		[Test]
		public void Should_not_fail_if_xml_is_missing_error_code()
		{
			const string validXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error><errorMessage>An error</errorMessage></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = validXml
			};

			var xmlSerializer = new ResponseDeserializer<TestObject>();

			var ex = Assert.Throws<ApiXmlException>(() => xmlSerializer.Deserialize(response));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.Error.ErrorMessage, Is.StringStarting("XML error parse failed"));
		}

		[Test]
		public void Should_not_fail_if_xml_is_missing_error_message()
		{
			const string validXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"123\"></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = validXml
			};

			var xmlSerializer = new ResponseDeserializer<TestObject>();

			var ex = Assert.Throws<ApiXmlException>(() => xmlSerializer.Deserialize(response));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.Error.ErrorMessage, Is.StringStarting("XML error parse failed"));
		}

		[Test]
		public void Should_throw_api_exception_with_null()
		{
			var apiXmlDeSerializer = new ResponseDeserializer<Status>();

			var apiException = Assert.Throws<ApiXmlException>(() => apiXmlDeSerializer.Deserialize(null));
			Assert.That(apiException.Message, Is.EqualTo("No response"));
		}

		[Test]
		public void Error_captures_http_status_code_from_html()
		{
			const string badXml = "<html><head>Error</head><body>It did not work<br><hr></body></html>";

			var response = new Response
				{
					StatusCode = HttpStatusCode.InternalServerError,
					Body = badXml
				};

			var xmlSerializer = new ResponseDeserializer<TestObject>();

			var ex = Assert.Throws<ApiXmlException>(() => xmlSerializer.Deserialize(response));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.StringStarting("Server error:"));
			Assert.That(ex.Message, Is.StringEnding(badXml));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
		}

		[Test]
		public void turns_html_ok_response_into_error()
		{
			const string badXml = "<html><head>Error</head><body>Some random html page<br><hr></body></html>";

			var response = new Response
				{
					StatusCode = HttpStatusCode.OK,
					Body = badXml
				};

			var xmlSerializer = new ResponseDeserializer<TestObject>();

			var ex = Assert.Throws<ApiXmlException>(() => xmlSerializer.Deserialize(response));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.StringStarting("Error trying to deserialize xml response"));
			Assert.That(ex.Message, Is.StringEnding(badXml));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
		}

		[Test]
		public void Should_handle_plaintext_OauthFail()
		{
			var response = new Response
				{
					StatusCode = HttpStatusCode.Unauthorized,
					Body = "OAuth authentication error: Not authorised - no user credentials provided"
				};

			var xmlSerializer = new ResponseDeserializer<TestObject>();
			var ex = Assert.Throws<ApiXmlException>(() => xmlSerializer.Deserialize(response));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.StringStarting("Error response"));
			Assert.That(ex.Message, Is.StringEnding(response.Body));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));

			Assert.That(ex.Error.Code, Is.EqualTo(9001));
			Assert.That(ex.Error.ErrorMessage, Is.EqualTo(response.Body));

		}
	}

}