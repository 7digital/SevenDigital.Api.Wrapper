using System.Net;

using NUnit.Framework;

using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests
{
	[ApiEndpoint("")]
	public class GeneralEndpoint
	{
	}

	[TestFixture]
	public class BaseUriTest
	{
		[Test]
		public async void Should_reach_google()
		{
			var googleResponse = await Api<GeneralEndpoint>.Create
				.UsingBaseUri("http://www.google.com")
				.Response();

			Assert.That(googleResponse.OriginalRequest.Url, Is.StringStarting("http://www.google.com"));
			Assert.That(googleResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}
	}
}
