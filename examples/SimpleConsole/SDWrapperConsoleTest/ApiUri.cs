using SevenDigital.Api.Wrapper;

namespace SDWrapperConsoleTest
{
	public class ApiUri : IApiUri
	{
		public string Uri
		{
			get { return "http://api.7digital.com/1.2"; }
		}

		public string SecureUri
		{
			get { return "https://api.7digital.com/1.2"; }
		}
	}
}