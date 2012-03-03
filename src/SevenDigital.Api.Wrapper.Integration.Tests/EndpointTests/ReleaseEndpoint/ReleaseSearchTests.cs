using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ReleaseEndpoint
{
	[TestFixture]
	public class ReleaseSearchTests
	{
		[Test]
		public void Can_hit_endpoint()
		{

			ReleaseSearch release = Api<ReleaseSearch>.Get
				.WithParameter("q", "no surprises")
				.WithParameter("type", ReleaseType.Single.ToString())
				.WithParameter("country", "GB")
				.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.Results.Count, Is.GreaterThan(0));
			Assert.That(release.Results.FirstOrDefault().Release.Type, Is.EqualTo(ReleaseType.Single));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			ReleaseSearch artistBrowse = Api<ReleaseSearch>.Get
				.WithParameter("q", "no surprises")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}

        [Test]
        public void Can_get_multiple_results()
        {
            var artistSearch = Api<ReleaseSearch>.Get
                .WithParameter("q", "pink")
                .WithParameter("page", "1")
                .WithParameter("pageSize", "20")
                .Please();

            Assert.That(artistSearch.Results.Count, Is.GreaterThan(1));

        }
	}
}