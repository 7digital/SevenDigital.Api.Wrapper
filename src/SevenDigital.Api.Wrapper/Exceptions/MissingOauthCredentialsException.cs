using System;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public class MissingOauthCredentialsException : Exception
	{
		public MissingOauthCredentialsException()
			: base("You need an implementation of IOAuthCredentials with a ConsumerKey and ConsumerSecret")
		{ }
	}
}