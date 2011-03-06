using System.Collections.Specialized;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http
{
	[TestFixture]
	public class NameValueExtensionsTests
	{
		[Test]
		public void Should_return_correct_nvc_as_querystring()
		{
			var nameValueCollection = new NameValueCollection(){{"artistId", "1234"},{"country", "GB"}};
			string querystring = nameValueCollection.ToQueryString();
			Assert.That(querystring, Is.EqualTo("artistId=1234&country=GB"));
		}
	}
}