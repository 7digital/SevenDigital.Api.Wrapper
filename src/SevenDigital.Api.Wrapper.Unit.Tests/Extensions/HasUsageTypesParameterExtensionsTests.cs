using NUnit.Framework;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Extensions
{
	[TestFixture]
	public class HasUsageTypesParameterExtensionsTests
	{
		private FluentApi<FakeUsageTypeEndpoint> _api;

		[SetUp]
		public void Setup()
		{
			_api = new FluentApi<FakeUsageTypeEndpoint>(null, null, null);
		}

		[Test]
		public void Adds_single_usageType()
		{
			_api.ForUsageTypes(UsageType.Download);

			Assert.That(_api.Parameters["usageTypes"], Is.EqualTo("download"));
		}

		[Test]
		public void Adds_multiple_usageTypes()
		{
			_api.ForUsageTypes(UsageType.Download, UsageType.SubscriptionStreaming, UsageType.AdSupportedStreaming);

			Assert.That(_api.Parameters["usageTypes"], Is.EqualTo("download,subscriptionstreaming,adsupportedstreaming"));
		}

		[ApiEndpoint("~/dummy")]
		private class FakeUsageTypeEndpoint : HasUsageTypesParameter
		{
		}
	}
}
