using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Exceptions
{
	[TestFixture]
	public class RemoteApiExceptionTests
	{
		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception()
		{
			var headers = new Dictionary<string, string>
				{
					{"Header1", "Header1Value"},
					{"Header2", "Header2Value"}
				};
			var response = new Response(HttpStatusCode.Conflict, headers, "responseBody");
			var inputException = new RemoteApiException("The server had an error", response, ErrorCode.UnexpectedInternalServerError);

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			Assert.That(outputException.ErrorCode, Is.EqualTo(inputException.ErrorCode));
			Assert.That(outputException.Headers, Is.EqualTo(inputException.Headers));
			Assert.That(outputException.ResponseBody, Is.EqualTo(inputException.ResponseBody));
			Assert.That(outputException.StatusCode, Is.EqualTo(inputException.StatusCode));
			Assert.That(outputException.Message, Is.EqualTo(inputException.Message));
		}
	}
}
