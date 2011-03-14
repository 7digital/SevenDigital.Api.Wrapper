using System;
using System.Security.Cryptography;
using System.Text;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.Authorization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.Authorization
{
	[TestFixture]
	public class HashComputerTests
	{
		[Test]
		public void Will_throw_argumentexception_if_null_hashalgorithm_passed()
		{
			var hashComputer = new HashComputer();

			var argumentException = Assert.Throws<ArgumentNullException>(() => hashComputer.Compute(null, ""));

			Assert.That(argumentException.ParamName, Is.EqualTo("hashAlgorithm"));
		}

		[Test]
		public void Will_throw_argumentexception_if_null_data_passed()
		{
			var hashComputer = new HashComputer();
			var hashAlgorithm = A.Fake<HashAlgorithm>();

			var argumentException = Assert.Throws<ArgumentNullException>(() => hashComputer.Compute(hashAlgorithm, ""));

			Assert.That(argumentException.ParamName, Is.EqualTo("data"));
		}

		[Test]
		public void Should_return_base64_version_of_hash()
		{
			const string hello = "Hello world!";
			var hashComputer = new HashComputer();
			var hashAlgorithm = HashAlgorithm.Create();
			byte[] expectedByte = Encoding.ASCII.GetBytes(hello);
			byte[] nonBase64Hash = hashAlgorithm.ComputeHash(expectedByte);

			string computedHash = hashComputer.Compute(hashAlgorithm, hello);

			Assert.That(nonBase64Hash, Is.Not.EqualTo(computedHash));
			Assert.That(Convert.ToBase64String(nonBase64Hash), Is.EqualTo(computedHash));
		}
	}
}