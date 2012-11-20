using System;
using NUnit.Framework;
using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests
{
	[TestFixture]
	public class StaticSetupCredentialPassingTests
	{
		[Test]
		public void Can_hit_endpoint_if_I_pass_credentials_into_static_method()
		{
			Status status = Api<Status>.CreateWithCreds(new AppSettingsCredentials(), new ApiUri()).MakeRequest().Please();

			Assert.That(status, Is.Not.Null);
			Assert.That(status.ServerTime.Day, Is.EqualTo(DateTime.Now.Day));
		}
	}
}