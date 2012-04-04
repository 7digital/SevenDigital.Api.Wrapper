using System.Collections.Generic;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http
{
	[TestFixture]
	public class HttpGetResolverTests
	{
		[Test]
		[Category("Integration")]
		public void Can_resolve_uri()
		{
			string apiUrl = "http://api.7digital.com/1.2";
			string consumerKey = new AppSettingsCredentials().ConsumerKey;
			string resolve = new HttpGetResolver().Resolve(string.Format("{0}/status?oauth_consumer_key={1}", apiUrl, consumerKey), "GET",
			                                               new Dictionary<string,string>());
			Assert.That(resolve, Is.Not.Empty);
		}
	}
}
