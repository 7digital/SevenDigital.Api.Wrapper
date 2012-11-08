using System;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Exceptions
{
	[TestFixture]
	public class ExceptionFactoryTests
	{
		[Test]
		public void Should_set_properties_when_creating_unrecognised_status_exception()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var result = ExceptionFactory.CreateUnrecognisedStatusException(dummyResponse);

			Assert.That(result.ResponseBody, Is.EqualTo(dummyResponse.Body));
			Assert.That(result.StatusCode, Is.EqualTo(dummyResponse.StatusCode));
		}

		[Test]
		public void Should_set_properties_when_creating_unrecognised_error_exception()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var innerException = new Exception();
			var result = ExceptionFactory.CreateUnrecognisedErrorException(dummyResponse, innerException);

			Assert.That(result.ResponseBody, Is.EqualTo(dummyResponse.Body));
			Assert.That(result.StatusCode, Is.EqualTo(dummyResponse.StatusCode));
			Assert.That(result.Message, Is.EqualTo(UnrecognisedErrorException.DEFAULT_ERROR_MESSAGE));
			Assert.That(result.InnerException, Is.SameAs(innerException));
		}

		[Test]
		public void Should_set_properties_when_creating_non_xml_response_exception()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var innerException = new Exception();
			var result = ExceptionFactory.CreateUnrecognisedErrorException(dummyResponse, innerException);

			Assert.That(result.ResponseBody, Is.EqualTo(dummyResponse.Body));
			Assert.That(result.StatusCode, Is.EqualTo(dummyResponse.StatusCode));
		}

		[Test]
		public void Should_set_properties_when_creating_non_api_error_exception()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var error = new Error() { Code = 1001 };
			var result = ExceptionFactory.CreateApiErrorException(error, dummyResponse);

			Assert.That(result.ResponseBody, Is.EqualTo(dummyResponse.Body));
			Assert.That(result.StatusCode, Is.EqualTo(dummyResponse.StatusCode));
			Assert.That(result.ErrorCode, Is.EqualTo(error.Code));
		}

		[Test]
		public void Should_set_properties_when_creating_oauth_exception()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var result = ExceptionFactory.CreateOAuthException(dummyResponse);

			Assert.That(result.ResponseBody, Is.EqualTo(dummyResponse.Body));
			Assert.That(result.StatusCode, Is.EqualTo(dummyResponse.StatusCode));
		}

		[Test]
		public void Should_create_invalid_parameter_exception_when_creating_non_api_error_exception_with_1xxx_code()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var error = new Error() { Code = 1001 };
			var result = ExceptionFactory.CreateApiErrorException(error, dummyResponse);

			Assert.That(result, Is.TypeOf<InputParameterException>());
		}

		[Test]
		public void Should_create_invalid_resource_exception_when_creating_non_api_error_exception_with_2xxx_code()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var error = new Error() { Code = 2001 };
			var result = ExceptionFactory.CreateApiErrorException(error, dummyResponse);

			Assert.That(result, Is.TypeOf<InvalidResourceException>());
		}

		[Test]
		public void Should_create_user_card_exception_when_creating_non_api_error_exception_with_3xxx_code()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var error = new Error() { Code = 3001 };
			var result = ExceptionFactory.CreateApiErrorException(error, dummyResponse);

			Assert.That(result, Is.TypeOf<UserCardException>());
		}

		[Test]
		public void Should_create_remote_api_exception_when_creating_non_api_error_exception_with_7xxx_code()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var error = new Error() { Code = 7001 };
			var result = ExceptionFactory.CreateApiErrorException(error, dummyResponse);

			Assert.That(result, Is.TypeOf<RemoteApiException>());
		}

		[Test]
		public void Should_create_remote_api_exception_when_creating_non_api_error_exception_with_9xxx_code()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var error = new Error() { Code = 9001 };
			var result = ExceptionFactory.CreateApiErrorException(error, dummyResponse);

			Assert.That(result, Is.TypeOf<RemoteApiException>());
		}

		[Test]
		public void Should_throw_unrecognised_error_exception_when_creating_non_api_error_exception_with_5xxx_code()
		{
			var dummyResponse = new Response(HttpStatusCode.BadRequest, "Test Body");
			var error = new Error() { Code = 5001 };

			Assert.Throws<UnrecognisedErrorException>(() => ExceptionFactory.CreateApiErrorException(error, dummyResponse));
		}
	}
}
