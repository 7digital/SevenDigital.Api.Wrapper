using System;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.LockerEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.Exceptions
{
	[TestFixture]
	public class ApiXmlExceptionTests
	{
		[Test]
		public void Should_fail_correctly_if_xml_error_returned()
		{
			// -- Deliberate error response
			Console.WriteLine("Trying artist/details without artistId parameter...");
			var apiXmlException = Assert.Throws<ApiXmlException>(() => Api<Artist>.Get.Please());

			Assert.That(apiXmlException.Error.Code, Is.EqualTo(1001));
			Assert.That(apiXmlException.Error.ErrorMessage, Is.EqualTo("Missing parameter artistId."));
		}

		[Test]
		public void Should_fail_correctly_if_non_xml_error_returned_eg_unauthorised()
		{
				// -- Deliberate unauthorized response
				Console.WriteLine("Trying user/locker without any credentials...");
				var apiXmlException = Assert.Throws<ApiXmlException>(() => Api<Locker>.Get.Please());
				Assert.That(apiXmlException.Error.Code, Is.EqualTo(9001));
				Assert.That(apiXmlException.Error.ErrorMessage, Is.EqualTo("OAuth authentication error: Not authorised - no user credentials provided"));
		}
	}
}