using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Tracks;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Tracks
{
	[TestFixture]
	public class TrackSearchTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<TrackSearch>.Create
				.WithParameter("q", "Happy");
			var trackSearch = await request.Please();

			Assert.That(trackSearch, Is.Not.Null);
			Assert.That(trackSearch.Results.Count, Is.GreaterThan(0));
			Assert.That(trackSearch.Results.FirstOrDefault().Type, Is.EqualTo(TrackType.track));
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<TrackSearch>.Create
				.WithParameter("q", "Happy")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20");
			var trackSearch = await request.Please();

			Assert.That(trackSearch, Is.Not.Null);
			Assert.That(trackSearch.Page, Is.EqualTo(2));
			Assert.That(trackSearch.PageSize, Is.EqualTo(20));
		}
	}
}