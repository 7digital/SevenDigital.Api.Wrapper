namespace SevenDigital.Api.Wrapper
{
	public class BaseUriProvider : IBaseUriProvider
	{
		private readonly string baseUri;

		public BaseUriProvider(string baseUri)
		{
			this.baseUri = baseUri;
		}

		public string BaseUri()
		{
			return baseUri;
		}
	}
}