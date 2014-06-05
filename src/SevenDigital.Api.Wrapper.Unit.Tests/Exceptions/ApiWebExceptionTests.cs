using System.Collections.Generic;
using System.Linq;
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

			Assert.That(outputException, Is.Not.Null);
			Assert.That(outputException.Message, Is.EqualTo(inputException.Message));
			Assert.That(outputException.InnerException, Is.InstanceOf<WebException>());
			Assert.That(outputException.InnerException.Message, Is.EqualTo(innerException.Message));
		}

		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception_with_original_request()
		{
			var inputException = MakeOriginalException();

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			var inputRequest = inputException.OriginalRequest;
			var outputRequest = outputException.OriginalRequest;

			Assert.That(outputRequest.Url, Is.EqualTo(inputRequest.Url));
			Assert.That(outputRequest.Method, Is.EqualTo(inputRequest.Method));
		}

		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception_request_headers()
		{
			var inputException = MakeOriginalException();

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			var inputRequest = inputException.OriginalRequest;
			var outputRequest = outputException.OriginalRequest;

			Assert.That(outputRequest.Headers, Is.Not.Null);
			Assert.That(outputRequest.Headers.Count, Is.EqualTo(inputRequest.Headers.Count));
			Assert.That(outputRequest.Headers.Keys.First(), Is.EqualTo(inputRequest.Headers.Keys.First()));
			Assert.That(outputRequest.Headers.Values.First(), Is.EqualTo(inputRequest.Headers.Values.First()));
		}

		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception_request_payload()
		{
			var inputException = MakeOriginalException();

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			var inputRequest = inputException.OriginalRequest;
			var outputRequest = outputException.OriginalRequest;

			Assert.That(outputRequest.Body, Is.Not.Null);
			Assert.That(outputRequest.Body.ContentType, Is.EqualTo(inputRequest.Body.ContentType));
			Assert.That(outputRequest.Body.Data, Is.EqualTo(inputRequest.Body.Data));
		}

		private static ApiWebException MakeOriginalException()
		{
			var innerException = new WebException("test");
			var requestBody = new RequestPayload("foo", "bar");
			var headers = new Dictionary<string, string> {{"key1", "value1"}};
			var originalRequest = new Request(HttpMethod.Get, "http://foo.com/bar?foo=bar",
				headers, requestBody);

			var inputException = new ApiWebException("request failed", innerException, originalRequest);
			return inputException;
		}
	}
}
