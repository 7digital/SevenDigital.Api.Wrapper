using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper
{
	public interface IApi
	{
		IFluentApi<T> Create<T>() where T : class, new();
	}
	
	public class ApiFactory: IApi
	{
		private readonly IApiUri _apiUri;
		private readonly IOAuthCredentials _oauthCredentials;

		public ApiFactory(IApiUri apiUri, IOAuthCredentials oauthCredentials)
		{
			_apiUri = apiUri;
			_oauthCredentials = oauthCredentials;
		}

		public IFluentApi<T> Create<T>() where T : class, new()
		{
			return new FluentApi<T>(new HttpClientMediator(), new RequestBuilder(new RouteParamsSubstitutor(_apiUri), _oauthCredentials), new ResponseParser(new ApiResponseDetector()));
		}
	}
}