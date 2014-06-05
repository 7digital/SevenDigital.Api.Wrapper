using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.TrackEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TrackEndpoint
{
	[TestFixture]
	public class TrackSearchTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			TrackSearch release = await Api<TrackSearch>.Create
				.WithParameter("q", "Happy")
				.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.Results.Count, Is.GreaterThan(0));
			Assert.That(release.Results.FirstOrDefault().Type, Is.EqualTo(TrackType.track));
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			TrackSearch artistBrowse = await Api<TrackSearch>.Create
				.WithParameter("q","Happy")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}