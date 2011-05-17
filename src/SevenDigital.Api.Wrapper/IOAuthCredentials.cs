namespace SevenDigital.Api.Wrapper
{
	public interface IOAuthCredentials
	{
		string ConsumerKey { get; set; }
		string ConsumerSecret { get; set; }
	}
}