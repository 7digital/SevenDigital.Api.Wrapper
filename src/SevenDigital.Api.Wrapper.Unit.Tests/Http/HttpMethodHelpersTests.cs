using System;
using System.Net.Http;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Http
{
	[TestFixture]
	public class HttpMethodHelpersTests
	{
		[Test]
		public void Should_parse_get()
		{
			var method = HttpMethodHelpers.Parse("GET");

			Assert.That(method, Is.EqualTo(HttpMethod.Get));
		}

		[Test]
		public void Should_parse_all_supported_http_methods()
		{
			Assert.That(HttpMethodHelpers.Parse("GET"), Is.EqualTo(HttpMethod.Get));
			Assert.That(HttpMethodHelpers.Parse("POST"), Is.EqualTo(HttpMethod.Post));
			Assert.That(HttpMethodHelpers.Parse("PUT"), Is.EqualTo(HttpMethod.Put));
			Assert.That(HttpMethodHelpers.Parse("DELETE"), Is.EqualTo(HttpMethod.Delete));
		}

		[Test]
		public void Should_parse_any_casing()
		{
			Assert.That(HttpMethodHelpers.Parse("Get"), Is.EqualTo(HttpMethod.Get));
			Assert.That(HttpMethodHelpers.Parse("post"), Is.EqualTo(HttpMethod.Post));
			Assert.That(HttpMethodHelpers.Parse("pUT"), Is.EqualTo(HttpMethod.Put));
			Assert.That(HttpMethodHelpers.Parse("DeLeTe"), Is.EqualTo(HttpMethod.Delete));
		}

		[Test]
		public void Should_not_parse_other_strings_as_http_method()
		{
			Assert.Throws<ArgumentException>(() => HttpMethodHelpers.Parse(""));
			Assert.Throws<ArgumentException>(() => HttpMethodHelpers.Parse("Soap"));
			Assert.Throws<ArgumentException>(() => HttpMethodHelpers.Parse("This is not a http method"));
		}
	}
}
