using NUnit.Framework;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ReleaseEndpoint
{
	[TestFixture]
	public class ReleaseEditorialTests
	{
		[Test, Ignore("Not yet public")]
		public async void Should_hit_endpoint()
		{
			var request = Api<ReleaseEditorial>
				.Create
				.ForReleaseId(815219);
			var editorial = await request.Please();

			Assert.That(editorial, Is.Not.Null);
			Assert.That(editorial.Review, Is.Not.Null);
			Assert.That(editorial.Review.Text, Is.Not.Empty);
		}
	}
}
