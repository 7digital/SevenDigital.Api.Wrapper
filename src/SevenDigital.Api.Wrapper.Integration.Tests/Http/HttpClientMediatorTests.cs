using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Xml;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Integration.Tests.Http
{
	[TestFixture]
	public class HttpClientMediatorTests
	{
		private const string API_URL = "http://api.7digital.com/1.2";
		private string _consumerKey;

		[SetUp]
		public void Setup()
		{
			_consumerKey = new AppSettingsCredentials().ConsumerKey;
		}

		[Test]
		public async void Can_resolve_uri()
		{
			var url = string.Format("{0}/status?oauth_consumer_key={1}", API_URL, _consumerKey);
			var request = new Request(HttpMethod.Get, url, new Dictionary<string, string>(), null, null);

			var response = await new HttpClientMediator().Send(request);
			AssertResponse(response, HttpStatusCode.OK);
		}

		[Test]
		public async void Can_resolve_uri_that_returns_gzip()
		{
			var url = string.Format("{0}/release/details?oauth_consumer_key={1}&releaseId=12345", API_URL, _consumerKey);
			var request = new Request(HttpMethod.Get, url, new Dictionary<string, string>(), null, null);

			var response = await new HttpClientMediator().Send(request);
			AssertResponse(response, HttpStatusCode.OK);
			AssertCanParseBody(response);
		}

		private static void AssertCanParseBody(Response response)
		{
			try
			{
				var doc = new XmlDocument();
				doc.LoadXml(response.Body);
				Assert.That(doc.SelectSingleNode("/response"), Is.Not.Null);
			}
			catch (XmlException ex)
			{
				Assert.Fail("Could not parse api response body as xml :{0}", ex.Message);
			}
		}

		[Test]
		public async void Bad_url_should_return_not_found()
		{
			var url = string.Format("{0}/foo/bar/fish/1234?oauth_consumer_key={1}", API_URL, _consumerKey);
			var request = new Request(HttpMethod.Get, url, new Dictionary<string, string>(), null, null);

			var response = await new HttpClientMediator().Send(request);
			AssertResponse(response, HttpStatusCode.NotFound);
		}

		[Test]
		public async void No_key_should_return_unauthorized()
		{
			var url = string.Format("{0}/status", API_URL);
			var request = new Request(HttpMethod.Get, url, new Dictionary<string, string>(), null, null);

			var response = await new HttpClientMediator().Send(request);
			AssertResponse(response, HttpStatusCode.Unauthorized);
		}

		[Test]
		[Ignore("There was a NullReferenceException that this test catches, however we don't enable this by default because:" +
		        "1: It would slow down the build a lot." +
		        "2: It would depend on a hanging-web.app being set up for the test.")]
		public async void Can_cope_with_timeouts()
		{
			const string apiUrl = "http://hanging-web-app.7digital.local";
			var request = new Request(HttpMethod.Post, apiUrl, new Dictionary<string, string>(), null, null);

			var response = await new HttpClientMediator().Send(request);
			AssertResponse(response, HttpStatusCode.OK);
		}


		[Test]
		public async void bad_url_post__should_return_not_found()
		{
			var url = string.Format("{0}/foo/bar/fish/1234?oauth_consumer_key={1}", API_URL, _consumerKey);
			var parameters = new Dictionary<string, string>
			{
				{"foo", "bar"}
			};

			var queryString = parameters.ToQueryString();
			var requestPayload = new RequestPayload("application/xml", queryString);
			var request = new Request(HttpMethod.Post, url, new Dictionary<string, string>(), requestPayload, null);

			var response = await new HttpClientMediator().Send(request);
			AssertResponse(response, HttpStatusCode.NotFound);
		}

		[Test]
		public async void Should_populate_OriginalRequest_property_with_the_request_passed_to_Send_method()
		{
			var url = string.Format("{0}/foo/bar/fish/1234?oauth_consumer_key={1}", API_URL, _consumerKey);
			var parameters = new Dictionary<string, string>
			{
				{"foo", "bar"}
			};

			var queryString = parameters.ToQueryString();
			var requestPayload = new RequestPayload("application/xml", queryString);
			var expectedHeaders = new Dictionary<string, string>
			{
				{"headerKey", "headerValue"}
			};
			const string expectedTraceId = "CUSTOM_TRACE_ID";
			var originalRequest = new Request(HttpMethod.Post, url, expectedHeaders, requestPayload, expectedTraceId);

			var response = await new HttpClientMediator().Send(originalRequest);

			Assert.That(response.OriginalRequest.Url, Is.EqualTo(url));
			Assert.That(response.OriginalRequest.Headers, Is.EqualTo(expectedHeaders));
			Assert.That(response.OriginalRequest.Body, Is.EqualTo(requestPayload));
			Assert.That(response.OriginalRequest.Method, Is.EqualTo(HttpMethod.Post));
			Assert.That(response.OriginalRequest.TraceId, Is.EqualTo(expectedTraceId));
		}

		private static void AssertResponse(Response response, HttpStatusCode expectedCode)
		{
			Assert.That(response, Is.Not.Null, "No response");
			Assert.That(response.StatusCode, Is.EqualTo(expectedCode), "Unexpected http status code");
			Assert.That(response.Headers.Count, Is.GreaterThan(0), "No headers found");
			Assert.That(response.Body, Is.Not.Empty, "No response body found");
		}
	}
}