using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.OAuth
{
	[TestFixture]
	public class NonceFactoryConcurrenyTests
	{
		[Test]
		public void Should_get_different_nonces_over_different_threads()
		{
			var list = new List<string>();
			Parallel.For(1, 10000, i =>
			{
				var generateNonce = NonceGenerator.ThreadSafeNonce();
				Assert.That(int.Parse(generateNonce), Is.GreaterThanOrEqualTo(0));
				Assert.That(list.Contains(generateNonce), Is.False, generateNonce + " appears twice: Failed at " + i);
				list.Add(generateNonce);
				Console.WriteLine(generateNonce);
			});
		}

		[Test]
		[Explicit("This is just to show that the old method can produce duplicate nonces")]
		public void Shows_non_thread_safe_will_get_same_nonces_over_different_threads()
		{
			var list = new List<string>();
			var shouldPass = false;
			Parallel.For(1, 1000, i =>
			{
				var generateNonce = NonceGenerator.OriginalNonceMethod();
				if (list.Contains(generateNonce))
				{
					Console.WriteLine(generateNonce + " appears twice: Failed at " + i);
					shouldPass = true;
				}
				list.Add(generateNonce);
				Console.WriteLine(generateNonce);
			});
			Assert.True(shouldPass, "There were no duplicate nonces!!");
		}
	}
}