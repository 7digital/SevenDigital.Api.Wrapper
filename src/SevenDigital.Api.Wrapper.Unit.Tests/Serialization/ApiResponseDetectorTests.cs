﻿using NUnit.Framework;
using SevenDigital.Api.Wrapper.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Serialization
{
	[TestFixture]
	public class ApiResponseDetectorTests
	{
		private const string DUMMY_HTML = "<html><head><title>Hello world</title></head><body><h1>Hello, test</h1></body></html>";
		private const string OK_RESPONSE = "<?xml><response status=\"ok\"></response>";
		private const string ERROR_RESPONSE = "<?xml><response status=\"error\"></response>";
		private const string OAUTH_ERROR = "OAuth authentication error: Access to resource denied";

		private IApiResponseDetector _apiResponseDetector;

		[SetUp]
		public void Setup()
		{
			_apiResponseDetector = new ApiResponseDetector();
		}

		[Test]
		public void Should_detect_xml()
		{
			var result = _apiResponseDetector.IsXml("<?xml><tag></tag>");

			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_not_detect_text_as_xml()
		{
			var result = _apiResponseDetector.IsXml("fish");

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_not_detect_html_as_xml()
		{
			var result = _apiResponseDetector.IsXml(DUMMY_HTML);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_detect_ok_response()
		{
			var result = _apiResponseDetector.IsApiOkResponse(OK_RESPONSE);

			Assert.That(result, Is.True);			
		}

		[Test]
		public void Should_not_detect_error_as_ok_response()
		{
			var result = _apiResponseDetector.IsApiOkResponse(ERROR_RESPONSE);

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
		public void Should_detect_500_as_server_error()
		{
			var result = _apiResponseDetector.IsServerError(500);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_detect_502_as_server_error()
		{
			var result = _apiResponseDetector.IsServerError(502);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_not_detect_200_as_server_error()
		{
			var result = _apiResponseDetector.IsServerError(200);

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
