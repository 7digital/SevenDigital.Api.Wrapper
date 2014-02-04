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

		public Response Send(Request request)
		{
			return _fakeResponse;
		}
	}
}