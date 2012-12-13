using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Exceptions
{
	[TestFixture]
	public class ApiWebExceptionTests
	{
		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception()
		{
			var innerException = new WebException("test");
			var inputException = new ApiWebException("request failed", "http://notconnected.com",innerException);

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			Assert.That(outputException.Uri, Is.EqualTo(inputException.Uri));
			Assert.That(outputException.Message, Is.EqualTo(inputException.Message));
			Assert.That(outputException.InnerException, Is.InstanceOf<WebException>());
			Assert.That(outputException.InnerException.Message, Is.EqualTo(innerException.Message));
		}
	}
}
