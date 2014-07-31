using NUnit.Framework;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Responses.Parsing
{
	[TestFixture]
	public class ApiResponseDetectorTests
	{
		private const string DUMMY_HTML = "<html><head><title>Hello world</title></head><body><h1>Hello, test</h1></body></html>";
		private const string HTML_WITH_UNBALANCED_TAGS = "<html><head><title>Hello world</title></head><body><P>Hello, test<p>para 2<hr></body></html>";
		
		private const string OK_RESPONSE = "<?xml version=\"1.0\"?><response status=\"ok\"></response>";
		private const string OK_RESPONSE_WITHOUT_DECL = "<response status=\"ok\"></response>";

		private const string ERROR_RESPONSE = "<?xml version=\"1.0\"?><response status=\"error\"></response>";
		private const string OK_RESPONSE_WITH_ERROR_STATUS_IN_CHILD = "<?xml version=\"1.0\"><response status=\"ok\"><child status=\"error\" /></response>";
		private const string OAUTH_ERROR = "OAuth authentication error: Access to resource denied";

		private const string SIMPLE_XML_WITH_DECL = "<?xml version=\"1.0\"?><tag></tag>";
		private const string SIMPLE_XML_WITHOUT_DECL = "<tag></tag>";

		private IApiResponseDetector _apiResponseDetector;

		[SetUp]
		public void Setup()
		{
			_apiResponseDetector = new ApiResponseDetector();
		}

		[Test]
		public void Should_detect_declaration_in_xml_with_declaration()
		{
			var result = _apiResponseDetector.StartsWithXmlDeclaration(SIMPLE_XML_WITH_DECL);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_not_detect_declaration_in_xml_without_declaration()
		{
			var result = _apiResponseDetector.StartsWithXmlDeclaration(SIMPLE_XML_WITHOUT_DECL);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_not_detect_xml_declaration_in_text()
		{
			var result = _apiResponseDetector.StartsWithXmlDeclaration("fish");

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_not_detect_xml_declaration_in_html()
		{
			var result = _apiResponseDetector.StartsWithXmlDeclaration(DUMMY_HTML);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_detect_xml_with_declaration()
		{
			var result = _apiResponseDetector.IsWellFormedXml(SIMPLE_XML_WITH_DECL);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_detect_xml_without_declaration()
		{
			var result = _apiResponseDetector.IsWellFormedXml(SIMPLE_XML_WITHOUT_DECL);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_not_detect_text_as_xml()
		{
			var result = _apiResponseDetector.IsWellFormedXml("fish");

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_not_detect_unbalanced_html_as_xml()
		{
			var result = _apiResponseDetector.IsWellFormedXml(HTML_WITH_UNBALANCED_TAGS);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_detect_ok_response()
		{
			var result = _apiResponseDetector.IsApiOkResponse(OK_RESPONSE);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_detect_ok_response_without_decl()
		{
			var result = _apiResponseDetector.IsApiOkResponse(OK_RESPONSE_WITHOUT_DECL);

			Assert.That(result, Is.True);
		}


		[Test]
		public void Should_not_detect_error_as_ok_response()
		{
			var result = _apiResponseDetector.IsApiOkResponse(ERROR_RESPONSE);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_not_detect_an_error_status_in_a_child_node()
		{
			var result = _apiResponseDetector.IsApiErrorResponse(OK_RESPONSE_WITH_ERROR_STATUS_IN_CHILD);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_not_detect_html_as_ok_response()
		{
			var result = _apiResponseDetector.IsApiOkResponse(DUMMY_HTML);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_detect_error_response()
		{
			var result = _apiResponseDetector.IsApiErrorResponse(ERROR_RESPONSE);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_not_detect_ok_as_error_response()
		{
			var result = _apiResponseDetector.IsApiErrorResponse(OK_RESPONSE);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_not_detect_html_as_error_response()
		{
			var result = _apiResponseDetector.IsApiErrorResponse(DUMMY_HTML);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_detect_oauth_error()
		{
			var result = _apiResponseDetector.IsOAuthError(OAUTH_ERROR);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_not_detect_oauth_error_for_xml()
		{
			var result = _apiResponseDetector.IsOAuthError(OK_RESPONSE);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_not_detect_oauth_error_for_html()
		{
			var result = _apiResponseDetector.IsOAuthError(DUMMY_HTML);

			Assert.That(result, Is.False);
		}
	}
}
