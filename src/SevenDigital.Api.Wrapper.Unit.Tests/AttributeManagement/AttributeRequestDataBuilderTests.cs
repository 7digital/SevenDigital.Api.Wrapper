using NUnit.Framework;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.AttributeManagement;

namespace SevenDigital.Api.Wrapper.Unit.Tests.AttributeManagement
{
	[TestFixture]
	public class AttributeRequestDataBuilderTests
	{
		[Test]
		public void Sets_correct_uri_based_on_apiEndpoint()
		{
			var requestData = AttributeRequestDataBuilder.BuildRequestData<StubEndpoint>();

			Assert.That(requestData.UriPath, Is.EqualTo("me/endpoint"));
		}

		[Test]
		public void Sets_IsSigned_if_OAuthSigned_specified()
		{
			var requestData = AttributeRequestDataBuilder.BuildRequestData<StubSecureEndpoint>();

			Assert.That(requestData.IsSigned);
		}

		[Test]
		public void Sets_IsSigned_false_if_OAuthSigned_not_specified()
		{
			var requestData = AttributeRequestDataBuilder.BuildRequestData<StubEndpoint>();

			Assert.That(requestData.IsSigned, Is.False);
		}

		[Test]
		public void Sets_UseHttps_if_RequireSecure_specified()
		{
			var requestData = AttributeRequestDataBuilder.BuildRequestData<StubSecureEndpoint>();

			Assert.That(requestData.UseHttps);
		}

		[Test]
		public void Sets_UseHttps_false_if_RequireSecure_not_specified()
		{
			var requestData = AttributeRequestDataBuilder.BuildRequestData<StubEndpoint>();

			Assert.That(requestData.UseHttps, Is.False);
		}

		[Test]
		public void Sets_HttpMethod_to_POST_if_HttpPost_specified()
		{
			var requestData = AttributeRequestDataBuilder.BuildRequestData<StubPostEndpoint>();

			Assert.That(requestData.HttpMethod, Is.EqualTo("POST"));
		}

		[Test]
		public void HttpMethod_defaults_to_GET_if_HttpPost_not_specified()
		{
			var requestData = AttributeRequestDataBuilder.BuildRequestData<StubEndpoint>();

			Assert.That(requestData.HttpMethod, Is.EqualTo("GET"));
		}
	}

	[ApiEndpoint("me/secure/endpoint")]
	[OAuthSigned]
	[RequireSecure]
	public class StubSecureEndpoint
	{}

	[ApiEndpoint("me/endpoint")]
	public class StubEndpoint
	{}

	[ApiEndpoint("me/post/endpoint")]
	[HttpPost]
	public class StubPostEndpoint
	{}
}
