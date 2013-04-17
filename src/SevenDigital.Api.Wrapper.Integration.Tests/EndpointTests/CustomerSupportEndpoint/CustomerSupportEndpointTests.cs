using System.Collections.Generic;
using NUnit.Framework;
using SevenDigital.Api.Schema.CustomerSupport;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.CustomerSupportEndpoint
{
	[TestFixture]
	public class CustomerSupportEndpointTests
	{
		[Test]
		public void Should_generate_endpoint_url()
		{
			var api = new FluentApi<CustomerSupport>();
			Assert.That(api.EndpointUrl, Is.EqualTo("https://api.7digital.com/1.2/customersupport"));
		}

		[Test]
		public void Should_generate_parameters()
		{
			var api = new FluentApi<CustomerSupport>();
			api.WithParameter("Email", "web-devteam-testing@7digital.com")
				.WithParameter("Message", "Integration test")
				.WithParameter("ClientName", "Web Team")
				.WithParameter("AdditionalClientInfo", "")
				.WithParameter("PartnerId", "0")
				.WithParameter("ShopId", "34");

			Assert.That(api.Parameters, Contains.Item(new KeyValuePair<string, string>("Email", "web-devteam-testing@7digital.com")));
			Assert.That(api.Parameters, Contains.Item(new KeyValuePair<string, string>("ClientName", "Web Team")));
			Assert.That(api.Parameters, Contains.Item(new KeyValuePair<string, string>("Message", "Integration test")));
			Assert.That(api.Parameters, Contains.Item(new KeyValuePair<string, string>("AdditionalClientInfo", "")));
			Assert.That(api.Parameters, Contains.Item(new KeyValuePair<string, string>("PartnerId", "0")));
			Assert.That(api.Parameters, Contains.Item(new KeyValuePair<string, string>("ShopId", "34")));
		}
	}
}
