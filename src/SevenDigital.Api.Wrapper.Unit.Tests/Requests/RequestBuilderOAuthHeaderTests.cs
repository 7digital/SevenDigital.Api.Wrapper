using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests
{
	[TestFixture]
	public class RequestBuilderOAuthHeaderTests
	{
		private RequestBuilder _requestBuilder;

		[SetUp]
		public void SetUp()
		{
			var appSettingsCredentials = new StubbedConsumerCreds();
			var apiUri = new ApiUri();
			_requestBuilder = new RequestBuilder(new RouteParamsSubstitutor(apiUri), appSettingsCredentials);
		}

		[Test]
		[Description("http://oauth.net/core/1.0/#rfc.section.5.4.1")]
		public void should_return_Authorization_OAuth_headers_as_outlined_in_OAuthspec_if_endpoint_requires_signature_without_usertoken()
		{
			var requestData = new RequestData
				{
					RequiresSignature = true
				};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.Not.Empty);
			Assert.That(actual, Is.StringStarting("OAuth"));
			Assert.That(actual, Is.StringContaining("oauth_consumer_key="));
			Assert.That(actual, Is.StringContaining("oauth_signature_method="));
			Assert.That(actual, Is.StringContaining("oauth_signature="));
			Assert.That(actual, Is.StringContaining("oauth_timestamp="));
			Assert.That(actual, Is.StringContaining("oauth_nonce="));
			Assert.That(actual, Is.StringContaining("oauth_version="));
		}

		[Test]
		public void should_not_return_oauth_token_if_Token_not_provided()
		{
			var requestData = new RequestData
				{
					RequiresSignature = true
				};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.Not.StringContaining("oauth_token="));
		}

		[Test]
		public void should_return_oauth_token_if_Token_provided()
		{
			var requestData = new RequestData
				{
					RequiresSignature = true,
					OAuthToken = "TOKEN",
					OAuthTokenSecret = "SECRET"
				};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.StringContaining("oauth_token="));
		}

		[Test]
		public void should_not_do_oauth_if_not_required()
		{
			var requestData = new RequestData
				{
					RequiresSignature = false
				};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.Not.StringContaining("oauth_token"));
			Assert.That(actual, Is.StringContaining("oauth_consumer_key="));
		}

		[Test]
		public void Should_include_request_params()
		{
			var requestData = new RequestData
			{
				HttpMethod = HttpMethod.Get,
				Endpoint = "http://api.com/endpoint",
				Parameters = new Dictionary<string, string>
					{
						{ "a", "b" },
						{ "c", "d" }
					},
				RequiresSignature = true,
				OAuthToken = "TOKEN",
				OAuthTokenSecret = "SECRET",
				Payload = null
			};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.StringContaining("oauth_token="));
		}

		[Test]
		public void Should_include_form_url_encoded_post_body()
		{
			var requestData = new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Endpoint = "http://api.com/endpoint",
				Payload = new RequestPayload("application/x-www-form-urlencoded", "a=b&c=d"),
				RequiresSignature = true,
				OAuthToken = "TOKEN",
				OAuthTokenSecret = "SECRET"
			};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.StringContaining("oauth_token="));
		}

		[Test]
		public void Should_not_include_json_post_body()
		{
			var requestData = new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Endpoint = "http://api.com/endpoint",
				Payload = new RequestPayload("application/json", "{ a: 1, c: 2 }"),
				RequiresSignature = true,
				OAuthToken = "TOKEN",
				OAuthTokenSecret = "SECRET"
			};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.StringContaining("oauth_token="));
		}

		private string GetAuthHeader(RequestData requestData)
		{
			var request = _requestBuilder.BuildRequest(requestData);

			return request.Headers["Authorization"];
		}
	}

	public class StubbedConsumerCreds : IOAuthCredentials
	{
		public string ConsumerKey
		{
			get { return "TOKEN"; }
		}

		public string ConsumerSecret
		{
			get { return "SECRET"; }
		}
	}
}
