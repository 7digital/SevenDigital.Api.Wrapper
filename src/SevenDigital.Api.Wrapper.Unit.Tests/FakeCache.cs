using System.Collections.Generic;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	internal class FakeCache: IResponseCache
	{
		public int SetCount { get; set; }
		public int TryGetCount { get; set; }
		public IList<Response> CachedResponses { get; set; }

		public Response StubResponse { get; set; }

		internal FakeCache()
		{
			CachedResponses = new List<Response>();
		}

		public void Set(RequestData key, Response value)
		{
			SetCount++;
			CachedResponses.Add(value);
		}

		public bool TryGet(RequestData key, out Response value)
		{
			TryGetCount++;
			value = StubResponse;
			return (StubResponse != null);
		}
	}
}