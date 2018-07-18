using NUnit.Framework;
using SevenDigital.Api.Wrapper.Http;
using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Http
{
	[TestFixture]
	public class HttpClientHandlerFactoryTests
	{
		[Test]
		public void Factory_returns_HttpClientHandler()
		{
			var factory = new HttpClientHandlerFactory();
			var result = factory.CreateHandler();

			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.TypeOf<HttpClientHandler>());
		}
	}
}
