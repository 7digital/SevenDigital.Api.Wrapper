using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Schema.OAuth;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.OAuth
{
	[TestFixture]
	public class OAuthRequestTokenTest
	{
		[Test]
		public void Should_throw_unauthorised_exception_if_no_oath_creds_passed()
		{
			var webException = Assert.Throws<WebException>(() => new FluentApi<OathRequestToken>().Please());
			Assert.That(webException.Message, Is.EqualTo("The remote server returned an error: (401) Unauthorized."));
		}
	}
}
