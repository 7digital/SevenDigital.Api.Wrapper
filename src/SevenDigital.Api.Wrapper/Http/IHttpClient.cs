using System;

namespace SevenDigital.Api.Wrapper.Http
{
	public interface IHttpClient
	{
		Response Get(GetRequest request);
		Response Post(PostRequest request);
	}
}