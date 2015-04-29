using System;
using System.Net;
using System.Runtime.Caching;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Responses
{
	public class InMemoryResponseCache : IResponseCache
	{
		private readonly CacheHeaderReader _cacheHeaderReader = new CacheHeaderReader();
		private readonly ObjectCache _objectCache;

		public InMemoryResponseCache() : this(MemoryCache.Default)
		{
		}

		public InMemoryResponseCache(ObjectCache objectCache)
		{
			_objectCache = objectCache;
		}

		public void Set(Response response, object value)
		{
			if (response.StatusCode >= HttpStatusCode.InternalServerError)
			{
				return;
			}

			if (value == null)
			{
				return;
			}

			var cacheExpiration = _cacheHeaderReader.GetExpiration(response);
			if (cacheExpiration.HasValue)
			{
				var cacheKey = MakeCacheKey(response.OriginalRequest, value.GetType());
				_objectCache.Set(cacheKey, value, cacheExpiration.Value);
			}
		}

		public bool TryGet<T>(Request request, out T value)
		{
			var cacheKey = MakeCacheKey(request, typeof(T));
			var cacheValue = _objectCache.Get(cacheKey);

			if (cacheValue is T)
			{
				value = (T)cacheValue;
				return true;
			}

			value = default(T);
			return false;
		}

		private string MakeCacheKey(Request request, Type storedType)
		{
			return "7d_" + storedType.Name + "_" + request.Method + "_" + request.Url;
		}
	}
}
