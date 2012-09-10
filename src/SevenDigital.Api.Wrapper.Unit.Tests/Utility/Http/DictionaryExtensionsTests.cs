using System.Collections.Generic;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http
{
	[TestFixture]
	public class DictionaryExtensionsTests
	{
		[Test]
		public void Should_return_correct_nvc_as_querystring()
		{
			var expected =  new Dictionary<string,string> { {"artistId", "1234"},{"country", "GB"}};
			string querystring = expected.ToQueryString();
			Assert.That(querystring, Is.EqualTo("artistId=1234&country=GB"));
		}

		[Test]
		public void Should_urlencode_if_specified_return_correct_nvc_as_querystring()
		{
			var expected = new Dictionary<string, string> { { "q", "Alive & Amplified" }, { "country", "GB" } };
			string querystring = expected.ToQueryString(true);
			Assert.That(querystring, Is.EqualTo("q=Alive%20%26%20Amplified&country=GB"));
		}

		[Test]
		public void Should_not_urlencode_if_specified_return_correct_nvc_as_querystring()
		{
			var expected = new Dictionary<string, string> { { "q", "ou est tu" }, { "country", "GB" } };
			string querystring = expected.ToQueryString();
			Assert.That(querystring, Is.EqualTo("q=ou est tu&country=GB"));
		}
	}
}