using System.Configuration;

namespace SevenDigital.Api.Wrapper.Integration.Tests
{
	public class ApiUri : IApiUri
	{
		public string Uri
		{
			get { return ConfigurationManager.AppSettings["Wrapper.BaseUrl"]; }
		}

		public string SecureUri
		{
			get { return ConfigurationManager.AppSettings["Wrapper.SecureBaseUrl"]; }
		}
	}
}