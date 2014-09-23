using System.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Releases;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests
{
	[TestFixture]
	public class ResponseAcceptTests
	{
		[Test]
		public async void Response_with_JsonPreferred_should_be_Ok_status()
		{
			var request = RequestARelease()
				.WithAccept(AcceptFormat.JsonPreferred);

			var response = await request.Response();

			Assert.That(response, Is.Not.Null);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public async void Response_with_JsonPreferred_should_be_json()
		{
			var request = RequestARelease()
				.WithAccept(AcceptFormat.JsonPreferred);

			var response = await request.Response();

			Assert.That(response.Body, Is.Not.StringContaining("xml"));
			Assert.That(response.Body, Is.Not.StringContaining("<"));
			Assert.That(response.Body, Is.StringContaining("{"));

			dynamic json = JObject.Parse(response.Body);
			Assert.That(json, Is.Not.Null);
		}

		[Test]
		public async void Response_with_JsonPreferred_should_have_json_ContentType()
		{
			var request = RequestARelease()
				.WithAccept(AcceptFormat.JsonPreferred);

			var response = await request.Response();

			Assert.That(response.Headers.ContainsKey("Content-Type"), Is.True);
			Assert.That(response.Headers["Content-Type"], Is.StringStarting("application/json"));

			Assert.That(response.ContentTypeIsJson(), Is.True);
		}

		[Test]
		public async void Response_with_XmlOnly_should_have_xml_ContentType()
		{
			var request = RequestARelease()
				.WithAccept(AcceptFormat.XmlOnly);

			var response = await request.Response();

			Assert.That(response, Is.Not.Null);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(response.Headers.ContainsKey("Content-Type"), Is.True);
			Assert.That(response.Headers["Content-Type"], Is.StringStarting("application/xml"));

			Assert.That(response.ContentTypeIsJson(), Is.False);
		}

		[Test]
		public async void Response_with_default_accept_should_have_xml_ContentType()
		{
			var request = RequestARelease();
			var response = await request.Response();

			Assert.That(response, Is.Not.Null);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(response.Headers.ContainsKey("Content-Type"), Is.True);
			Assert.That(response.Headers["Content-Type"], Is.StringStarting("application/xml"));

			Assert.That(response.ContentTypeIsJson(), Is.False);
		}

		private IFluentApi<Release> RequestARelease()
		{
			return Api<Release>.Create
				.ForReleaseId(1685647)
				.WithParameter("country", "GB");
		}
	}
}
