using System;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class CredentialChecker
	{
		private CredentialChecker() {}

		public static CredentialChecker Instance 
		{
			get
			{
				throw new NotSupportedException("Windows Phone 7 doesn't support reflection on calling assemblies, so you need to inject the Endpoint Resolver with your credentials");
			}
		}

		public IOAuthCredentials Credentials
		{
			get { return null; }
		}
	}
}
