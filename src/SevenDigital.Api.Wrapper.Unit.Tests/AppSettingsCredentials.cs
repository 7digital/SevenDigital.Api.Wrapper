using System.Configuration;
using SevenDigital.Api.Wrapper.Environment;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	public class AppSettingsCredentials : IOAuthCredentials
	{
		public AppSettingsCredentials()
		{
			ConsumerKey = "UNIT_TEST";
			ConsumerSecret = "";
		}

		public string ConsumerKey { get; set; }
		public string ConsumerSecret { get; set; }
	}
}