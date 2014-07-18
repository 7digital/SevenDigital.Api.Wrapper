using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper
{
	public class StringBaseUriProvider : IBaseUriProvider
	{
		private readonly string _baseUri;

		public StringBaseUriProvider(string baseUri)
		{
			_baseUri = baseUri;
		}

		public string BaseUri(RequestData requestData)
		{
			return _baseUri;
		}
	}

	public class ApiBaseUriProvider : IBaseUriProvider
	{
		private readonly IApiUri _apiUri;

		public ApiBaseUriProvider(IApiUri apiUri)
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