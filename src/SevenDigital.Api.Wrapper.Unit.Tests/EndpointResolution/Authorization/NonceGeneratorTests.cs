using System;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.Authorization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.Authorization
{
	[TestFixture]
	public class NonceGeneratorTests
	{
		[Test]
		public void Should_generate_nonce()
		{
			var nonceGenerator = new NonceGenerator();
			string generate = nonceGenerator.Generate();
			Assert.That(generate, Is.Not.Empty);

			int nonce = Convert.ToInt32(generate);
			Assert.That(nonce, Is.GreaterThanOrEqualTo(123400));
			Assert.That(nonce, Is.LessThanOrEqualTo(9999999));
		}
	}
}