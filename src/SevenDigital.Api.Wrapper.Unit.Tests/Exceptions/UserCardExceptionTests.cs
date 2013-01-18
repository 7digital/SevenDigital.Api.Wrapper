using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Exceptions
{
	[TestFixture]
	public class UserCardExceptionTests
	{
		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception()
		{
			var headers = new Dictionary<string, string>
				{
					{"Header1", "Header1Value"},
					{"Header2", "Header2Value"}
				};
			var response = new Response(HttpStatusCode.OK, headers, "responseBody");

			var inputException = new UserCardException("user card has expired", response, ErrorCode.UserCardHasExpired);

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			Assert.That(outputException.Headers, Is.EqualTo(inputException.Headers));
			Assert.That(outputException.ResponseBody, Is.EqualTo(inputException.ResponseBody));
			Assert.That(outputException.StatusCode, Is.EqualTo(inputException.StatusCode));
			Assert.That(outputException.Message, Is.EqualTo(inputException.Message));
		}
	}
}
