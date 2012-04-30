using System;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IHttpClient
	{
		IResponse Get(IRequest request);
		void GetAsync(IRequest request, Action<IResponse> callback);

		IResponse Post(IRequest request);
		void PostAsync(IRequest request, Action<IResponse> callback);
	}
}