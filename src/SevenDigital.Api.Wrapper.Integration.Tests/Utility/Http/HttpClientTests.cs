using System.Collections.Generic;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.Utility.Http
{
	[TestFixture]
	public class HttpClientTests
	{
		[Test]
		public void Can_resolve_uri()
		{
			var apiUrl = "http://api.7digital.com/1.2";
			var consumerKey = new AppSettingsCredentials().ConsumerKey;
			var request = new Request(string.Format("{0}/status?oauth_consumer_key={1}", apiUrl, consumerKey),
									  new Dictionary<string, string>());

			var response = new HttpClient().Get(request);
			Assert.That(response.Body, Is.Not.Empty);
			Assert.That(response.Headers.Count, Is.GreaterThan(0));
		}
	}
}
