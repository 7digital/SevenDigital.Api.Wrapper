using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Exceptions
{
	[TestFixture]
	public class UnexpectedXmlResponseExceptionTests
	{
		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception()
		{
			var headers = new Dictionary<string, string>
				{
					{"Header1", "Header1Value"},
					{"Header2", "Header2Value"}
				};
			var response = new Response(HttpStatusCode.GatewayTimeout, headers, "responseBody");
			var innerException = new Exception();

			var inputException = new UnexpectedXmlResponseException(innerException, response);

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			Assert.That(outputException.Headers, Is.EqualTo(inputException.Headers));
			Assert.That(outputException.ResponseBody, Is.EqualTo(inputException.ResponseBody));
			Assert.That(outputException.StatusCode, Is.EqualTo(inputException.StatusCode));
			Assert.That(outputException.Message, Is.EqualTo(inputException.Message));
		}
	}
}
