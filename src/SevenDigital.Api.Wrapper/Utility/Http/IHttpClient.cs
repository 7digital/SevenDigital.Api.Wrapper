using System;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IHttpClient
	{
		IResponse<string> Get(IRequest request);
		void GetAsync(IRequest request, Action<IResponse<string>> callback);

		IResponse<string> Post(IRequest request, string data);
		void PostAsync(IRequest request, string data, Action<IResponse<string>> callback);
	}
}