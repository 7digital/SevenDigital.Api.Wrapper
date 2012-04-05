using System;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IHttpClient
	{
		IResponse Get(IRequest request);
		void GetAsync(IRequest request, Action<IResponse> callback);

		IResponse Post(IRequest request, string data);
		void PostAsync(IRequest request, string data, Action<IResponse> callback);
	}
}