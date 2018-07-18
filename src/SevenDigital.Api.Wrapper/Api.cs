using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper
{
	public interface IApi
	{
		IFluentApi<T> Create<T>() where T : class, new();
	}

	public class ApiFactory : IApi
	{
		private readonly IApiUri _apiUri;
		private readonly IHttpClientHandlerFactory _httpClientHandlerFactory;
		private readonly IOAuthCredentials _oauthCredentials;

		public ApiFactory(IApiUri apiUri, IOAuthCredentials oauthCredentials)
			: this(apiUri, oauthCredentials, new HttpClientHandlerFactory())
		{
		}

		public ApiFactory(
			IApiUri apiUri,
			IOAuthCredentials oauthCredentials,
			IHttpClientHandlerFactory httpClientHandlerFactory)
		{
			_apiUri = apiUri;
			_oauthCredentials = oauthCredentials;
			_httpClientHandlerFactory = httpClientHandlerFactory;
		}

		public IFluentApi<T> Create<T>() where T : class, new()
		{
			return new FluentApi<T>(
				new HttpClientMediator(_httpClientHandlerFactory),
				new RequestBuilder(new RouteParamsSubstitutor(_apiUri), _oauthCredentials),
				new ResponseParser(new ApiResponseDetector()));
		}
	}
}
