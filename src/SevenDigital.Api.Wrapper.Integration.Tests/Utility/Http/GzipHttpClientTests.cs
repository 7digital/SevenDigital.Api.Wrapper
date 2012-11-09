using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Xml;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.Utility.Http
{
	[TestFixture]
	public class GzipHttpClientTests
	{
		private const string API_URL = "http://api.7digital.com/1.2";
		private readonly TimeSpan _asyncTimeout = new TimeSpan(0, 0, 0, 20);
		private string _consumerKey;

		[SetUp]
		public void Setup()
		{
			_consumerKey = new AppSettingsCredentials().ConsumerKey;
		}

		[Test]
		public void Can_resolve_uri()
		{
			var url = string.Format("{0}/status?oauth_consumer_key={1}", API_URL, _consumerKey);
			var request = new GetRequest(url, new Dictionary<string, string>());

			var response = new GzipHttpClient().Get(request);
			AssertResponse(response, HttpStatusCode.OK);
		}

		[Test]
		public void Can_resolve_uri_that_returns_gzip()
		{
			var url = string.Format("{0}/release/details?oauth_consumer_key={1}&releaseId=12345", API_URL, _consumerKey);
			var request = new GetRequest(url, new Dictionary<string, string>());

			var response = new GzipHttpClient().Get(request);
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
		public void Can_resolve_uri_async()
		{
			var url = string.Format("{0}/status?oauth_consumer_key={1}", API_URL, _consumerKey);
			var request = new GetRequest(url, new Dictionary<string, string>());

			var autoResetEvent = new AutoResetEvent(false);
			Response response = null;

			Action<Response> callback = callbackResponse =>
			                            {
			                            	response = callbackResponse;
			                            	autoResetEvent.Set();
			                            };

			new GzipHttpClient().GetAsync(request, callback);

			var signalled = autoResetEvent.WaitOne(_asyncTimeout);
			Assert.That(signalled, Is.True, "event was not signalled");

			AssertResponse(response, HttpStatusCode.OK);
		}

		[Test]
		public void Bad_url_should_return_not_found()
		{
			var url = string.Format("{0}/foo/bar/fish/1234?oauth_consumer_key={1}", API_URL, _consumerKey);
			var request = new GetRequest(url, new Dictionary<string, string>());

			var response = new GzipHttpClient().Get(request);
			AssertResponse(response, HttpStatusCode.NotFound);
		}

		[Test]
		public void Bad_url_should_return_not_found_async()
		{
			var url = string.Format("{0}/foo/bar/fish/1234?oauth_consumer_key={1}", API_URL, _consumerKey);
			var request = new GetRequest(url, new Dictionary<string, string>());

			var autoResetEvent = new AutoResetEvent(false);
			Response response = null;

			Action<Response> callback = callbackResponse =>
			                            {
			                            	response = callbackResponse;
			                            	autoResetEvent.Set();
			                            };

			new GzipHttpClient().GetAsync(request, callback);

			var signalled = autoResetEvent.WaitOne(_asyncTimeout);
			Assert.That(signalled, Is.True, "event was not signalled");

			AssertResponse(response, HttpStatusCode.NotFound);
		}

		[Test]
		public void No_key_should_return_unauthorized()
		{
			var url = string.Format("{0}/status", API_URL);
			var request = new GetRequest(url, new Dictionary<string, string>());

			var response = new GzipHttpClient().Get(request);
			AssertResponse(response, HttpStatusCode.Unauthorized);
		}

		[Test]
		[Ignore("There was a NullReferenceException that this test catches, however we don't enable this by default because:" +
		        "1: It would slow down the build a lot." +
		        "2: It would depend on a hanging-web.app being set up for the test.")]
		public void Can_cope_with_timeouts()
		{
			const string apiUrl = "http://hanging-web-app.7digital.local";
			var request = new GetRequest(apiUrl, new Dictionary<string, string>());

			var response = new GzipHttpClient().Get(request);
			AssertResponse(response, HttpStatusCode.OK);
		}


		[Test]
		public void bad_url_post__should_return_not_found()
		{
			var url = string.Format("{0}/foo/bar/fish/1234?oauth_consumer_key={1}", API_URL, _consumerKey);
			var parameters = new Dictionary<string, string>
			                 {
			                 	{"foo", "bar"}
			                 };

			var request = new PostRequest(url, new Dictionary<string, string>(), parameters);

			var response = new GzipHttpClient().Post(request);
			AssertResponse(response, HttpStatusCode.NotFound);
		}

		[Test]
		public void bad_url_post_should_return_not_found_async()
		{
			string url = string.Format("{0}/foo/bar/fish/1234?oauth_consumer_key={1}", API_URL, _consumerKey);
			var parameters = new Dictionary<string, string>
			                 {
			                 	{"foo", "bar"}
			                 };

			var request = new PostRequest(url, new Dictionary<string, string>(), parameters);

			var autoResetEvent = new AutoResetEvent(false);
			Response response = null;

			Action<Response> callback = callbackResponse =>
			                            {
			                            	response = callbackResponse;
			                            	autoResetEvent.Set();
			                            };

			new GzipHttpClient().PostAsync(request, callback);

			var signalled = autoResetEvent.WaitOne(_asyncTimeout);
			Assert.That(signalled, Is.True, "event was not signalled");

			AssertResponse(response, HttpStatusCode.NotFound);
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