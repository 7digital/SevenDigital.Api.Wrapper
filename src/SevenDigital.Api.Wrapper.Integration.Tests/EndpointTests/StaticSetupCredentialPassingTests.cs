using System;
using NUnit.Framework;
using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests
{
	[TestFixture]
	public class StaticSetupCredentialPassingTests
	{
		[Test]
		public async void Can_hit_endpoint_if_I_pass_credentials_into_static_method()
		{
			Status status = await Api<Status>.CreateWithCreds(new AppSettingsCredentials(), new ApiUri()).Please();

			Assert.That(status, Is.Not.Null);
			Assert.That(status.ServerTime.Day, Is.EqualTo(DateTime.Now.Day));
		}
	}
}