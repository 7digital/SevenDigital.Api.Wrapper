using NUnit.Framework;
using SevenDigital.Api.Wrapper.Repository;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointTests
{
	[TestFixture]
	[Category("Integration")]
	public class ReleaseDetailsTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver()); // TODO: Set up using castle?

			Release release = new FluentApi<Release>(httpGetResolver)
				.WithParameter("releaseId", "155408")
				.WithParameter("country", "GB")
				.Resolve();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.Title, Is.EqualTo("Dreams"));
			Assert.That(release.Artist.Name, Is.EqualTo("The Whitest Boy Alive"));
		}
	}
}