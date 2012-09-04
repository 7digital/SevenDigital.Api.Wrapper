using System;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IHttpClient
	{
		Response Get(Request request);
		void GetAsync(Request request, Action<Response> callback);

		Response Post(Request request);
		void PostAsync(Request request, Action<Response> callback);
	}
}