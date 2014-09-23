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
		public async void Response_With_JsonPreferred_ShouldBeOk()
		{
			var request = Api<Release>.Create
				.ForReleaseId(1685647)
				.WithParameter("country", "GB")
				.WithAccept(AcceptFormat.JsonPreferred);

			var response = await request.Response();

			Assert.That(response, Is.Not.Null);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public async void Response_With_JsonPreferred_ShouldBeJson()
		{
			var request = Api<Release>.Create
				.ForReleaseId(1685647)
				.WithParameter("country", "GB")
				.WithAccept(AcceptFormat.JsonPreferred);

			var response = await request.Response();

			Assert.That(response.Body, Is.Not.StringContaining("xml"));
			Assert.That(response.Body, Is.Not.StringContaining("<"));
			Assert.That(response.Body, Is.StringContaining("{"));

			dynamic json = JObject.Parse(response.Body);
			Assert.That(json, Is.Not.Null);
		}
	}
}
