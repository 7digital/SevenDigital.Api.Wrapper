using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Exceptions
{
	[TestFixture]
	public class MissingDependencyExceptionTests
	{
		[Test]
		public void Should_round_trip_serialise_and_deserialise_exception()
		{
			var inputException = new MissingDependencyException(typeof(string));

			var roundTripSerialiser = new RoundTripSerialiser();
			var outputException = roundTripSerialiser.RoundTrip(inputException);

			Assert.That(outputException.Message, Is.EqualTo(inputException.Message));
		}
	}
}
