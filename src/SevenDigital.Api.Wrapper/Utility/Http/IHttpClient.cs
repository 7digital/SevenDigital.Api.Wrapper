using System;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IHttpClient
	{
		Response Get(IRequest request);
		void GetAsync(IRequest request, Action<Response> callback);

		Response Post(IRequest request);
		void PostAsync(IRequest request, Action<Response> callback);
	}
}