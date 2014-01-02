using System;
using System.Configuration;

namespace SevenDigital.Api.Wrapper.Integration.Tests
{
	public class TestDataFromEnvironmentOrAppSettings
	{
		public static string AccessToken
		{
			get
			{
				return Environment.GetEnvironmentVariable("WRAPPER_INTEGRATION_TEST_ACCESS_TOKEN") ??
					ConfigurationManager.AppSettings["Integration.Tests.AccessToken"];
			}
		}
		public static string AccessTokenSecret
		{
			get
			{
				return Environment.GetEnvironmentVariable("WRAPPER_INTEGRATION_TEST_ACCESS_TOKEN_SECRET") ??
					ConfigurationManager.AppSettings["Integration.Tests.AccessTokenSecret"];
			}
		}
	}
}