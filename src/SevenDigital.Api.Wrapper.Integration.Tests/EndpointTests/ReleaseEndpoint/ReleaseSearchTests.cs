using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ReleaseEndpoint
{
	[TestFixture]
	public class ReleaseSearchTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<ReleaseSearch>.Create
				.WithParameter("q", "no surprises")
				.WithParameter("type", ReleaseType.Single.ToString())
				.WithParameter("country", "GB");
			var releaseSearch = await request.Please();

			Assert.That(releaseSearch, Is.Not.Null);
			Assert.That(releaseSearch.Results.Count, Is.GreaterThan(0));
			Assert.That(releaseSearch.Results.FirstOrDefault().Release.Type, Is.EqualTo(ReleaseType.Single));
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<ReleaseSearch>.Create
				.WithParameter("q", "no surprises")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20");
			var releaseSearch = await request.Please();

			Assert.That(releaseSearch, Is.Not.Null);
			Assert.That(releaseSearch.Page, Is.EqualTo(2));
			Assert.That(releaseSearch.PageSize, Is.EqualTo(20));
		}

		[Test]
		public async void Can_get_multiple_results()
		{
			var request = Api<ReleaseSearch>.Create
				.WithParameter("q", "pink")
				.WithParameter("page", "1")
				.WithParameter("pageSize", "20");
			var releaseSearch = await request.Please();

			Assert.That(releaseSearch.Results.Count, Is.GreaterThan(1));
		}
	}
}