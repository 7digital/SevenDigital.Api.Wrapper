using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Exceptions
{
	[TestFixture]
	public class ApiWebExceptionTests
	{
		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception()
		{
			var innerException = new WebException("test");
			var inputException = new ApiWebException("request failed", innerException);

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			Assert.That(outputException.Message, Is.EqualTo(inputException.Message));
			Assert.That(outputException.InnerException, Is.InstanceOf<WebException>());
			Assert.That(outputException.InnerException.Message, Is.EqualTo(innerException.Message));
		}

		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception_with_originalRequest()
		{
			var innerException = new WebException("test");
			var originalRequest = new Request(HttpMethod.Get, "http://foo.com/bar?foo=bar", new Dictionary<string, string>{{"key", "value"}});
			var inputException = new ApiWebException("request failed", innerException, originalRequest);

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			Assert.That(outputException.OriginalRequest.Url, Is.EqualTo(originalRequest.Url));
			Assert.That(outputException.OriginalRequest.Method, Is.EqualTo(originalRequest.Method));
			Assert.That(outputException.OriginalRequest.Headers.Count, Is.EqualTo(originalRequest.Headers.Count));
		}
	}
}
