using System.Configuration;

namespace SevenDigital.Api.Wrapper.Integration.Tests
{
	public class TestDataFromEnvironmentOrAppSettings
	{
		public static string AccessToken
		{
			get
			{
				return ValueFromEnvOrConfig("WRAPPER_INTEGRATION_TEST_ACCESS_TOKEN", "Integration.Tests.AccessToken");
			}
		}
		public static string AccessTokenSecret
		{
			get
			{
				return ValueFromEnvOrConfig("WRAPPER_INTEGRATION_TEST_ACCESS_TOKEN_SECRET", "Integration.Tests.AccessTokenSecret");
			}
		}

		private static string ValueFromEnvOrConfig(string envName, string configName)
		{
			return System.Environment.GetEnvironmentVariable(envName) ?? ConfigurationManager.AppSettings[configName];
		}
	}
}