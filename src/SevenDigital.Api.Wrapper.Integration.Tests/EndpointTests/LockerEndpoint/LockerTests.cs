using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Schema.LockerEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.LockerEndpoint
{
    [TestFixture]
    public class LockerTests
    {
        private const string TOKEN = "user token";
        private const string TOKEN_SECRET = "user secret";

        [Test, Ignore("can't run this test without a valid user token/secret, need to think on how we do this...")]
        public void TestName()
        {
            var locker = new FluentApi<Locker>(new EndpointResolver(new HttpGetResolver(), new UrlSigner(), new AppSettingsCredentials()))
                .ForUser(TOKEN, TOKEN_SECRET)
                .Please();

            Assert.That(locker.LockerReleases.Count, Is.GreaterThan(0));
        }
    }
}