using System.Configuration;
using SevenDigital.Api.Wrapper;

namespace SDWrapperConsoleTest
{
	public class ApiCredentials : IOAuthCredentials
	{
		public ApiCredentials()
		{
			ConsumerKey = ConfigurationManager.AppSettings["7digital.ConsumerKey"];
			ConsumerSecret = ConfigurationManager.AppSettings["7digital.ConsumerSecret"];
		}

		public string ConsumerKey { get; private set; }
		public string ConsumerSecret { get; private set; }
	}
}