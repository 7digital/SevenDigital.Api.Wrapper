namespace SevenDigital.Api.Wrapper
{
	public interface IOAuthCredentials
	{
		string ConsumerKey { get; }
		string ConsumerSecret { get; }
	}
}