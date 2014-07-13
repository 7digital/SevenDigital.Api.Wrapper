using System.Net;

using NUnit.Framework;

using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests
{
	[ApiEndpoint("")]
	public class GoogleEndpoint : IBaseUriProvider
	{
		public string BaseUri()
		{
			return "http://www.google.com";
		}
	}

	[TestFixture]
	public class BaseUriTest
	{
		[Test]
		public async void Should_reach_google()
		{
			var googleResponse = await Api<GoogleEndpoint>.Create.Response();

			Assert.That(googleResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}
	}
}
