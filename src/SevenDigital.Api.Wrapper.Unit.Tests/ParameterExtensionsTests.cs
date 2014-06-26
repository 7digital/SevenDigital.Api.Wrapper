using NUnit.Framework;
using SevenDigital.Api.Schema.Baskets;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class ParameterExtensionsTests
	{
		[Test]
		public void Should_add_int_parameter()
		{
			var basketEndpoint = new FluentApi<AddItemToBasket>();
			basketEndpoint.WithParameter("answer", 42);

			Assert.That(basketEndpoint.Parameters.Keys.Contains("answer"));
			Assert.That(basketEndpoint.Parameters["answer"], Is.EqualTo("42"));
		}

		[Test]
		public void Should_add_decimal_parameter()
		{
			var basketEndpoint = new FluentApi<AddItemToBasket>();
			basketEndpoint.WithParameter("answer", 42.12m);

			Assert.That(basketEndpoint.Parameters.Keys.Contains("answer"));
			Assert.That(basketEndpoint.Parameters["answer"], Is.EqualTo("42.12"));
		}

	}
}
