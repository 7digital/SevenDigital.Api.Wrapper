using SevenDigital.Api.Wrapper.Environment;
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
		public IFluentApi<T> Create<T>() where T : class, new()
		{
			var apiUri = EssentialDependencyCheck<IApiUri>.Instance;
			var oAuthConsumerCredentials = EssentialDependencyCheck<IOAuthCredentials>.Instance;

			return new FluentApi<T>(new HttpClientMediator(), new RequestBuilder(apiUri, oAuthConsumerCredentials), new ResponseParser(new ApiResponseDetector()));
		}
	}
}