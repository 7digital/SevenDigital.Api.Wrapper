using System;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Schema.Lockers;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.Integration.Tests.Exceptions
{
	[TestFixture]
	public class ErrorConditionTests
	{
		[Test]
		public void Should_fail_with_input_parameter_exception_if_xml_error_returned()
		{
			// -- Deliberate error response
			Console.WriteLine("Trying artist/details without artistId parameter...");
			var request = Api<Artist>.Create;
			var apiXmlException = Assert.Throws<InputParameterException>(async () => await request.Please());

			Assert.That(apiXmlException.ErrorCode, Is.EqualTo(ErrorCode.RequiredParameterMissing));
			Assert.That(apiXmlException.Message, Is.StringStarting("Missing or invalid querystring value for artistId"));
		}

		[Test]
		public void Should_fail_with_oauth_exception_if_plaintext_error_returned_eg_unauthorised()
		{
			// -- Deliberate unauthorized response
			Console.WriteLine("Trying user/locker without any credentials...");
			var request = Api<Locker>.Create;
			var apiXmlException = Assert.Throws<OAuthException>(async () => await request.Please());
			Assert.That(apiXmlException.ResponseBody, Is.StringStarting("OAuth authentication error: "));
		}
	}
}