using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Translations;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TranslationsEndpoint
{
	[TestFixture]
	public class TranslationsEndpointTests
	{
		[Test, Ignore("Not yet public")]
		public void Should_hit_translations_endpoint()
		{
			var translations = Api<Translations>
				.Create
				.MakeRequest()
				.Please();

			Assert.That(translations, Is.Not.Null);
			Assert.That(translations.TotalItems, Is.GreaterThan(0));
			Assert.That(translations.TranslationItems.FirstOrDefault(), Is.Not.Null);
		}

		[Test, Ignore("Not yet public")]
		public void Should_hit_translations_endpoint_with_paging()
		{
			var translations = Api<Translations>
				.Create
				.MakeRequest()
				.WithPageSize(1)
				.WithPageNumber(2)
				.Please();

			Assert.That(translations, Is.Not.Null);
			Assert.That(translations.TotalItems, Is.GreaterThan(0));
			Assert.That(translations.TranslationItems.FirstOrDefault(), Is.Not.Null);
			Assert.That(translations.Page, Is.EqualTo(1));
			Assert.That(translations.PageSize, Is.EqualTo(2));
		}
	}
}
