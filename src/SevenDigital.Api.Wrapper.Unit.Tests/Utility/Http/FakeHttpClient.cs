using System;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http
{
	public class FakeHttpClient : IHttpClient
	{
		private readonly Response _fakeResponse;

		public FakeHttpClient()
		{
		}

		public FakeHttpClient(Response fakeResponse)
		{
			_fakeResponse = fakeResponse;
		}

		public Response Get(GetRequest request)
		{
			throw new NotImplementedException();
		}

		public void GetAsync(GetRequest request, Action<Response> callback)
		{
			callback(_fakeResponse);
		}

		public Response Post(PostRequest request)
		{
			throw new NotImplementedException();
		}

		public void PostAsync(PostRequest request, Action<Response> callback)
		{
			throw new NotImplementedException();
		}
	}
}