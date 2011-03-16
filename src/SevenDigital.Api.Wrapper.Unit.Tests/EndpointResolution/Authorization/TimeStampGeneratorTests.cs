using System;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.Authorization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.Authorization
{
	[TestFixture]
	public class TimeStampGeneratorTests
	{
		[Test]
		public void Should_generate_timestamp()
		{
			var timeStampGenerator = new TimeStampGenerator();
			string generate = timeStampGenerator.Generate();
			Assert.That(generate, Is.Not.Empty);

			int timestamp = Convert.ToInt32(generate);
			Assert.That(timestamp, Is.GreaterThan(0));
		}
	}
}