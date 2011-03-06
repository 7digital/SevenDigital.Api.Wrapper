using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointTests.ReleaseEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ReleaseSearchTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ReleaseSearch release = new FluentApi<ReleaseSearch>(httpGetResolver)
				.WithParameter("q", "no surprises")
				.WithParameter("type", ReleaseType.Video.ToString())
				.WithParameter("country", "GB")
				.Resolve();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.Results.Releases.Count, Is.GreaterThan(0));
			Assert.That(release.Results.Releases.FirstOrDefault().Type, Is.EqualTo(ReleaseType.Video));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver());

			ReleaseSearch artistBrowse = new FluentApi<ReleaseSearch>(httpGetResolver)
				.WithParameter("q", "no surprises")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Resolve();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}