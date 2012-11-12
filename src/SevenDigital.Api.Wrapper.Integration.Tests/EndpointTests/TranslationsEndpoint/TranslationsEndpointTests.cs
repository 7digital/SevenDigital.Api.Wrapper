using NUnit.Framework;
using SevenDigital.Api.Schema.Translations;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TranslationsEndpoint
{
	[TestFixture]
	public class TranslationsEndpointTests
	{
		[Test]
		public void Should_hit_translations_endpoint()
		{
			var translations = Api<Translations>
				.Create
				.WithParameter("shopId", "34")
				.WithParameter("defaultLanguageId", "1")
				.Please();

			Assert.That(translations, Is.Not.Null);
			Assert.That(translations.TotalItems, Is.GreaterThan(0));
		}

		[Test]
		public void Should_hit_translations_endpoint_with_paging()
		{
			var translations = Api<Translations>
				.Create
				.WithParameter("shopId", "34")
				.WithParameter("defaultLanguageId", "1")
				.WithPageSize(1)
				.WithPageNumber(1)
				.Please();

			Assert.That(translations, Is.Not.Null);
			Assert.That(translations.TotalItems, Is.GreaterThan(0));
			Assert.That(translations.Page, Is.EqualTo(1));
			Assert.That(translations.PageSize, Is.EqualTo(1));
		}
	}
}
