using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema.LockerEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.LockerEndpoint
{
	[TestFixture]
	public class LockerTests
	{
		[Test, Ignore("This will need a valid request token and secret!")]
		public void Should_return_a_users_locker()
		{
			try
			{
				const string userToken = "USER_TOKEN_HERE";
				const string userSecret = "USER_SECRET_HERE";
				var locker = Api<Locker>.Create
				                        .ForUser(userToken, userSecret)
				                        .Please();

				Assert.That(locker.Response.LockerReleases, Is.Not.Empty);

				var lockerRelease = locker.Response.LockerReleases.First();
				Assert.That(lockerRelease.Available, Is.True.Or.False);
				Assert.That(lockerRelease.Release, Is.Not.Null);
				Assert.That(lockerRelease.LockerTracks, Is.Not.Empty);
			}
			catch (WebException ex)
			{
				Assert.Fail(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
			}
		}
	}
}