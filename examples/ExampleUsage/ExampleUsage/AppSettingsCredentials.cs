using System.Configuration;

namespace SevenDigital.Api.Wrapper.ExampleUsage
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