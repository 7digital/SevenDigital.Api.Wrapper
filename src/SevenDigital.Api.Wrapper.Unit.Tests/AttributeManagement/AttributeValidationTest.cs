using NUnit.Framework;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.AttributeManagement;

namespace SevenDigital.Api.Wrapper.Unit.Tests.AttributeManagement
{
	[TestFixture]
	public class AttributeValidationTests
	{
		[Test]
		public void Sets_correct_uri_based_on_apiEndpoint()
		{
			var attributeValidation = new AttributeValidation<StubEndpoint>();
			var requestData = attributeValidation.Validate();

			Assert.That(requestData.UriPath, Is.EqualTo("me/endpoint"));
		}

		[Test]
		public void Sets_IsSigned_if_OAuthSigned_specified()
		{
			var attributeValidation = new AttributeValidation<StubSecureEndpoint>();
			var requestData = attributeValidation.Validate();

			Assert.That(requestData.IsSigned);
		}

		[Test]
		public void Sets_IsSigned_false_if_OAuthSigned_not_specified()
		{
			var attributeValidation = new AttributeValidation<StubEndpoint>();
			var requestData = attributeValidation.Validate();

			Assert.That(requestData.IsSigned, Is.False);
		}

		[Test]
		public void Sets_UseHttps_if_RequireSecure_specified()
		{
			var attributeValidation = new AttributeValidation<StubSecureEndpoint>();
			var requestData = attributeValidation.Validate();

			Assert.That(requestData.UseHttps);
		}

		[Test]
		public void Sets_UseHttps_false_if_RequireSecure_not_specified()
		{
			var attributeValidation = new AttributeValidation<StubEndpoint>();
			var requestData = attributeValidation.Validate();

			Assert.That(requestData.UseHttps, Is.False);
		}

		[Test]
		public void Sets_Httpethod_to_POST_if_HttpPost_specified()
		{
			var attributeValidation = new AttributeValidation<StubPostEndpoint>();
			var requestData = attributeValidation.Validate();

			Assert.That(requestData.HttpMethod, Is.EqualTo("POST"));
		}

		[Test]
		public void Httpethod_tdefaults_to_GET_if_HttpPost_not_specified()
		{
			var attributeValidation = new AttributeValidation<StubEndpoint>();
			var requestData = attributeValidation.Validate();

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
