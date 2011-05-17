using System.Configuration;
using SevenDigital.Api.Wrapper;

namespace SevenDigital.Api.Dynamic.Integration.Tests
{
	public class AppSettingsCredentials : IOAuthCredentials
	{
		public AppSettingsCredentials()
		{
			ConsumerKey = ConfigurationManager.AppSettings["Wrapper.ConsumerKey"];
			ConsumerSecret = ConfigurationManager.AppSettings["Wrapper.ConsumerSecret"];
		}

		public string ConsumerKey { get; set; }
		public string ConsumerSecret { get; set; }
	}
}