namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	public class AppSettingsCredentials : IOAuthCredentials
	{
		public AppSettingsCredentials()
		{
			ConsumerKey = "YOUR_KEY_HERE";
			ConsumerSecret = "";
		}

		public string ConsumerKey { get; set; }
		public string ConsumerSecret { get; set; }
	}
}