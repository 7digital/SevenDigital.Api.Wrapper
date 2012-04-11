using System;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http
{
	public class FakeHttpClient : IHttpClient
	{
		private readonly IResponse _fakeResponse;

		public FakeHttpClient()
		{
		}

		public FakeHttpClient(IResponse fakeResponse)
		{
			_fakeResponse = fakeResponse;
		}

		public IResponse Get(IRequest request)
		{
			throw new NotImplementedException();
		}

		public void GetAsync(IRequest request, Action<IResponse> callback)
		{
			callback(_fakeResponse);
		}

		public IResponse Post(IRequest request)
		{
			throw new NotImplementedException();
		}

		public void PostAsync(IRequest request, Action<IResponse> callback)
		{
			throw new NotImplementedException();
		}
	}
}