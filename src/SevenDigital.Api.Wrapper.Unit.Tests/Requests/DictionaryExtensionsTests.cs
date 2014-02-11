﻿using System.Collections.Generic;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests
{
	[TestFixture]
	public class DictionaryExtensionsTests
	{
		[Test]
		public void Should_urlencode_if_specified_return_correct_nvc_as_querystring()
		{
			var expected = new Dictionary<string, string> { { "q", "Alive & Amplified" }, { "country", "GB" } };
			string querystring = expected.ToQueryString();
			Assert.That(querystring, Is.EqualTo("q=Alive%20%26%20Amplified&country=GB"));
		}
	}
}