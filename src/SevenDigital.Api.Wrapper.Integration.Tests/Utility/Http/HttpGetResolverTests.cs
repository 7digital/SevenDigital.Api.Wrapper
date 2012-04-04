using System.Collections.Generic;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.Utility.Http
{
	[TestFixture]
	public class HttpGetResolverTests
	{
		[Test]
		public void Can_resolve_uri()
		{
			var apiUrl = "http://api.7digital.com/1.2";
			var consumerKey = new AppSettingsCredentials().ConsumerKey;
			var resolve = new HttpGetDispatcher().Dispatch(string.Format("{0}/status?oauth_consumer_key={1}", apiUrl, consumerKey),
														   new Dictionary<string, string>());
			Assert.That(resolve, Is.Not.Empty);
		}

		[Test]
		public void Can_access_headers()
		{
			var apiUrl = "http://api.7digital.com/1.2";
			var consumerKey = new AppSettingsCredentials().ConsumerKey;
			var resolve = new HttpGetDispatcher().FullDispatch(string.Format("{0}/status?oauth_consumer_key={1}", apiUrl, consumerKey),
														   new Dictionary<string, string>());
			Assert.That(resolve.Headers.Count, Is.GreaterThan(0));
			Assert.That(resolve.Headers["Expires"], Is.Not.Null);
		}
	}
}
