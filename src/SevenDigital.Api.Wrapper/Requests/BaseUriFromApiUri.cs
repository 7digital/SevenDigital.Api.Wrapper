namespace SevenDigital.Api.Wrapper.Requests
{
	public class BaseUriFromApiUri : IBaseUriProvider
	{
		private readonly IApiUri _apiUri;

		public BaseUriFromApiUri(IApiUri apiUri)
		{
			_apiUri = apiUri;
		}

		public string BaseUri(RequestData requestData)
		{
			if (requestData.UseHttps)
			{
				return _apiUri.SecureUri;
			}

			return _apiUri.Uri;
		}
	}
}