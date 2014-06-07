using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Responses
{
	[TestFixture]
	public class ResponseTests
	{
		[Test]
		public void Should_not_be_json_when_content_type_header_is_absent()
		{
			var headers = new Dictionary<string, string>();
			var response = new Response(HttpStatusCode.OK, headers, string.Empty);

			Assert.That(response.ContentTypeIsJson(), Is.False);
		}

		[Test]
		public void Should_not_be_json_when_content_type_header_is_xml()
		{
			var headers = new Dictionary<string, string>
				{
					{"Content-Type", "application/xml"}
				};
			var response = new Response(HttpStatusCode.OK, headers, string.Empty);

			Assert.That(response.ContentTypeIsJson(), Is.False);
		}

		[Test]
		public void Should_be_json_when_content_type_header_is_text_json()
		{
			var headers = new Dictionary<string, string>
				{
					{"Content-Type", "text/json"}
				};
			var response = new Response(HttpStatusCode.OK, headers, string.Empty);

			Assert.That(response.ContentTypeIsJson(), Is.True);
		}


		[Test]
		public void Should_be_json_when_content_type_header_is_application_json()
		{
			var headers = new Dictionary<string, string>
				{
					{"Content-Type", "application/json"}
				};
			var response = new Response(HttpStatusCode.OK, headers, string.Empty);

			Assert.That(response.ContentTypeIsJson(), Is.True);
		}

		[Test]
		public void Should_be_json_when_content_type_header_is_json_with_charset()
		{
			var headers = new Dictionary<string, string>
				{
					{"Content-Type", "application/json; charset=UTF-8"}
				};
			var response = new Response(HttpStatusCode.OK, headers, string.Empty);

			Assert.That(response.ContentTypeIsJson(), Is.True);
		}
	}
}