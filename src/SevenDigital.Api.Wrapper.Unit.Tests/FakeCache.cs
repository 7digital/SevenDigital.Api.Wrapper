using System.Collections.Generic;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	internal class FakeCache: IResponseCache
	{
		public int SetCount { get; set; }
		public int TryGetCount { get; set; }
		public IList<object> CachedResponses { get; set; }

		public object StubCachedObject { get; set; }

		internal FakeCache()
		{
			CachedResponses = new List<object>();
		}

		public void Set(Response response, object value)
		{
			SetCount++;
			CachedResponses.Add(value);
		}

		public bool TryGet<T>(Request request, out T value)
		{
			TryGetCount++;
			value = (T)StubCachedObject;
			return (StubCachedObject != null);
		}
	}
}