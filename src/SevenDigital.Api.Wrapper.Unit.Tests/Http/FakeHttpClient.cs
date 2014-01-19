using System;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Http
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

		public Response Post(PostRequest request)
		{
			throw new NotImplementedException();
		}
	}
}