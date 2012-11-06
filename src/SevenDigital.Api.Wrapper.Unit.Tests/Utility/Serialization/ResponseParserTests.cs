using System;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Serialization
{
	[TestFixture]
	public class ResponseParserTests
	{
		[Test]
		public void Should_throw_argument_null_exception_when_reponse_is_null()
		{
			var apiXmlDeserializer = new ResponseParser<TestObject>();

			Assert.Throws<ArgumentNullException>(() => apiXmlDeserializer.Parse(null));
		}

		[Test]
		public void Can_deserialize_object()
		{
			//success case with well formed response
			const string xml = "<?xml version=\"1.0\"?><response xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" status=\"ok\"><testObject id=\"1\"> <name>A big test object</name><listOfThings><string>one</string><string>two</string><string>three</string></listOfThings><listOfInnerObjects><InnerObject id=\"1\"><Name>Trevor</Name></InnerObject><InnerObject id=\"2\"><Name>Bill</Name></InnerObject></listOfInnerObjects></testObject></response>";

			var stubResponse = new Response
				{
					StatusCode = HttpStatusCode.OK,
					Body = xml
				};

			var xmlParser = new ResponseParser<TestObject>();

			Assert.DoesNotThrow(() => xmlParser.Parse(stubResponse));

			TestObject testObject = xmlParser.Parse(stubResponse);

			Assert.That(testObject.Id, Is.EqualTo(1));
		}

		[Test]
		public void Should_throw_input_parameter_exception_for_1001_error_code_with_error_http_status_code()
		{
			const string errorXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"1001\"><errorMessage>Test error</errorMessage></error></response>";
			var response = new Response
				{
					StatusCode = HttpStatusCode.InternalServerError,
					Body = errorXml
				};

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<InputParameterException>(() => xmlParser.Parse(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.EqualTo(errorXml));
			Assert.That(ex.Message, Is.EqualTo("Test error"));
			Assert.That(ex.ErrorCode, Is.EqualTo(1001));
		}

		[Test]
		public void Should_throw_input_parameter_exception_for_1001_error_code_with_ok_http_status_code()
		{
			const string errorXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"1001\"><errorMessage>Test error</errorMessage></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = errorXml
			};

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<InputParameterException>(() => xmlParser.Parse(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.EqualTo(errorXml));
			Assert.That(ex.Message, Is.EqualTo("Test error"));
			Assert.That(ex.ErrorCode, Is.EqualTo(1001));
		}

		[Test]
		public void Should_throw_invalid_resource_exception_for_2001_error_code_with_ok_http_status_code()
		{
			const string errorXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"2001\"><errorMessage>Test error</errorMessage></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = errorXml
			};

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<InvalidResourceException>(() => xmlParser.Parse(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.EqualTo(errorXml));
			Assert.That(ex.Message, Is.EqualTo("Test error"));
			Assert.That(ex.ErrorCode, Is.EqualTo(2001));
		}

		[Test]
		public void Should_throw_user_card_exception_for_3001_error_code_with_ok_http_status_code()
		{
			const string errorXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"3001\"><errorMessage>Test error</errorMessage></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = errorXml
			};

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<UserCardException>(() => xmlParser.Parse(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.EqualTo(errorXml));
			Assert.That(ex.Message, Is.EqualTo("Test error"));
			Assert.That(ex.ErrorCode, Is.EqualTo(3001));
		}

		[Test]
		public void Should_throw_remote_api_exception_for_9001_error_code_with_ok_http_status_code()
		{
			const string errorXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"9001\"><errorMessage>Test error</errorMessage></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = errorXml
			};

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<RemoteApiException>(() => xmlParser.Parse(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.EqualTo(errorXml));
			Assert.That(ex.Message, Is.EqualTo("Test error"));
			Assert.That(ex.ErrorCode, Is.EqualTo(9001));
		}

		[Test]
		public void Should_throw_unrecognised_error_exception_for_error_code_out_of_range_with_ok_http_status_code()
		{
			const string errorXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"42\"><errorMessage>Test error</errorMessage></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = errorXml
			};

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<UnrecognisedErrorException>(() => xmlParser.Parse(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.EqualTo(errorXml));
			Assert.That(ex.Message, Is.EqualTo("Test error"));
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

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<UnrecognisedErrorException>(() => xmlParser.Parse(response));

			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.EqualTo(response.Body));
			Assert.That(ex.Message, Is.EqualTo(UnrecognisedErrorException.DEFAULT_ERROR_MESSAGE));
		}

		[Test]
		public void Should_throw_non_xml_response_exception_when_response_body_is_null()
		{
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = null
			};

			var xmlSerializer = new ResponseParser<TestObject>();

			var ex = Assert.Throws<NonXmlResponseException>(() => xmlSerializer.Parse(response));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.Null);
		}

		[Test]
		public void Should_throw_non_xml_response_exception_when_response_body_is_empty()
		{
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = string.Empty
			};

			var xmlSerializer = new ResponseParser<TestObject>();

			var ex = Assert.Throws<NonXmlResponseException>(() => xmlSerializer.Parse(response));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.Empty);
		}

		[Test]
		public void Should_throw_unrecognised_error_exception_if_xml_is_missing_error_code()
		{
			const string validXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error><errorMessage>An error</errorMessage></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = validXml
			};

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<UnrecognisedErrorException>(() => xmlParser.Parse(response));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(ex.ResponseBody, Is.EqualTo(response.Body));
			Assert.That(ex.Message, Is.EqualTo(UnrecognisedErrorException.DEFAULT_ERROR_MESSAGE));
		}

		[Test]
		public void Should_throw_unrecognised_error_exception_if_xml_is_missing_error_message()
		{
			const string validXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"123\"></error></response>";
			var response = new Response
			{
				StatusCode = HttpStatusCode.OK,
				Body = validXml
			};

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<UnrecognisedErrorException>(() => xmlParser.Parse(response));
			Assert.That(ex.ResponseBody, Is.EqualTo(response.Body));
			Assert.That(ex.Message, Is.EqualTo(UnrecognisedErrorException.DEFAULT_ERROR_MESSAGE));
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

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<NonXmlResponseException>(() => xmlParser.Parse(response));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.EqualTo("Error deserializing xml response"));
			Assert.That(ex.ResponseBody, Is.EqualTo(badXml));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
		}

		[Test]
		public void Should_throw_non_xml_response_exception_for_html_ok_response()
		{
			const string badXml = "<html><head>Error</head><body>Some random html page<br><hr></body></html>";

			var response = new Response
				{
					StatusCode = HttpStatusCode.OK,
					Body = badXml
				};

			var xmlParser = new ResponseParser<TestObject>();

			var ex = Assert.Throws<NonXmlResponseException>(() => xmlParser.Parse(response));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.EqualTo("Error deserializing xml response"));
			Assert.That(ex.ResponseBody, Is.EqualTo(badXml));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
		}

		[Test]
		public void Should_handle_plaintext_oauth_fail()
		{
			const string ErrorText = "OAuth authentication error: Not authorised - no user credentials provided";
			var response = new Response
				{
					StatusCode = HttpStatusCode.Unauthorized,
					Body = ErrorText
				};

			var xmlParser = new ResponseParser<TestObject>();
			var ex = Assert.Throws<OAuthException>(() => xmlParser.Parse(response));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.EqualTo(ErrorText));
			Assert.That(ex.ResponseBody, Is.EqualTo(response.Body));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
		}

		[Test]
		public void Should_handle_plaintext_oauth_fail_with_ok_status()
		{
			const string ErrorText = "OAuth authentication error: Not found";

			var response = new Response
				{
					StatusCode = HttpStatusCode.OK,
					Body = ErrorText
				};

			var xmlParser = new ResponseParser<TestObject>();
			var ex = Assert.Throws<OAuthException>(() => xmlParser.Parse(response));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.EqualTo(ErrorText));
			Assert.That(ex.ResponseBody, Is.EqualTo(response.Body));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
		}

		[Test]
		public void Should_throw_unrecognised_status_exception_when_deserializing_with_invalid_status()
		{
			const string InvalidStatusXmlResponse = "<?xml version=\"1.0\"?><response status=\"fish\" version=\"1.2\"></response>";
			var response = new Response
				{
					StatusCode = HttpStatusCode.OK,
					Body = InvalidStatusXmlResponse
				};

			var xmlParser = new ResponseParser<TestEmptyObject>();

			var ex = Assert.Throws<UnrecognisedStatusException>(() => xmlParser.Parse(response));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.EqualTo(UnrecognisedStatusException.DEFAULT_ERROR_MESSAGE));
			Assert.That(ex.ResponseBody, Is.EqualTo(InvalidStatusXmlResponse));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
		}

		[Test]
		public void Should_throw_exception_when_deserialize_with_missing_status()
		{
			const string MissingStatusXmlResponse = "<?xml version=\"1.0\"?><response version=\"1.2\"></response>";
			var response = new Response
				{
					StatusCode = HttpStatusCode.OK,
					Body = MissingStatusXmlResponse
				};

			var xmlParser = new ResponseParser<TestEmptyObject>();

			var ex = Assert.Throws<UnrecognisedStatusException>(() => xmlParser.Parse(response));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.EqualTo(UnrecognisedStatusException.DEFAULT_ERROR_MESSAGE));
			Assert.That(ex.ResponseBody, Is.EqualTo(MissingStatusXmlResponse));
			Assert.That(ex.StatusCode, Is.EqualTo(response.StatusCode));
		}
	}
}