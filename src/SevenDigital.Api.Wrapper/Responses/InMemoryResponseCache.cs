using System;
using System.Runtime.Caching;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Responses
{
	public class InMemoryResponseCache : IResponseCache
	{
		private readonly ObjectCache _objectCache;

		public InMemoryResponseCache(ObjectCache objectCache)
		{
			_objectCache = objectCache;
		}

		public void Set(Response response, object value)
		{
			var cacheDuration = ResponseCachingReader.DurationSeconds(response);
			if (cacheDuration > 0)
			{
				var expiration = DateTimeOffset.Now.AddSeconds(cacheDuration);
				var cacheKey = MakeCacheKey(response.OriginalRequest);
				_objectCache.Set(cacheKey, value, expiration);
			}
		}

		public bool TryGet<T>(Request request, out T value)
		{
			var cacheKey = MakeCacheKey(request);
			var cacheValue = _objectCache.Get(cacheKey);

			if (cacheValue is T)
			{
				value = (T)cacheValue;
				return true;
			}

			value = default(T);
			return false;
		}

		private string MakeCacheKey(Request  request)
		{
			return "7d_" + request.Method + "_" + request.Url;
		}
	}
}
