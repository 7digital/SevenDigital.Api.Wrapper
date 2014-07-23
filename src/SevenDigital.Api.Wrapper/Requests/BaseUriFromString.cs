namespace SevenDigital.Api.Wrapper.Requests
{
	public class BaseUriFromString : IBaseUriProvider
	{
		private readonly string _baseUri;

		public BaseUriFromString(string baseUri)
		{
			_baseUri = baseUri;
		}

		public string BaseUri(RequestData requestData)
		{
			return _baseUri;
		}
	}
}