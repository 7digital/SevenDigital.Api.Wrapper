using System;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IHttpClient
	{
		Response Get(GetRequest request);
		void GetAsync(GetRequest request, Action<Response> callback);

		Response Post(PostRequest request);
		void PostAsync(PostRequest request, Action<Response> callback);
	}
}