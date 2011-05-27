namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public class OAuthCredentials : IOAuthCredentials
	{
		protected OAuthCredentials() { }

		public OAuthCredentials(string consumerKey, string consumerSecret)
		{
			ConsumerKey = consumerKey;
			ConsumerSecret = consumerSecret;
		}

		public string ConsumerKey { get; protected set; }
		public string ConsumerSecret { get; protected set; }
	}
}