# Response caching #

The 7digital Api Wrapper supports response caching via the `IResponseCache` interface.

We assume that you have already chosen a caching technology. This could be Memcached, redis etc, but in the example we will use the simplest one, [MemoryCache](http://msdn.microsoft.com/en-us/library/system.runtime.caching.memorycache(v=vs.110).aspx).

In order to cache responses you must supply an implementation of  `IResponseCache` that stores and retrieves data in your cache. Then you attach it to the `IFluentApi` with the `UsingCache` method.

Here is a sample implementation of a `IResponseCache` that stores values in a MemoryCache. This can be adapted to use a different underlying cache.


	using System;
	using System.Runtime.Caching;
	using SevenDigital.Api.Wrapper.Requests;
	using SevenDigital.Api.Wrapper.Responses;

	namespace SevenDigital.Api.Wrapper.ExampleUsage
	{
		public class MemoryResponseCache : IResponseCache
		{
			private readonly MemoryCache _memoryCache;

			public MemoryResponseCache(MemoryCache memoryCache)
			{
				_memoryCache = memoryCache;
			}

			public void Set(RequestData key, object value)
			{
				var memoryCacheKey = MakeCacheKey(key);
				_memoryCache.Set(memoryCacheKey, value, DefaultExpiration());
			}

			public bool TryGet<T>(RequestData key, out T value)
			{
				var cacheKey = MakeCacheKey(key);
				var cacheValue = _memoryCache.Get(cacheKey);

				if (cacheValue is T)
				{
					value = (T)cacheValue;
					return true;
				}

				value = default(T);
				return false;
			}

			private string MakeCacheKey(RequestData key)
			{
				var paramValues = string.Join("_", key.Parameters);
				return "7digital_" + key.HttpMethod + "_" + key.Endpoint + "_" + paramValues;
			}

			private DateTimeOffset DefaultExpiration()
			{
				return DateTimeOffset.Now.AddMinutes(10);
			}
		}
	}



An instance of this `IResponseCache` should then be attached to every `IFluentApi` instance. A good place to do this is in the code that creates them, i.e. in a customised `IApi` class in the example below. 

For simplicity's sake, this example wraps the default `IApi` implementation and just adds the cache:

		public class ExampleApi : IApi
		{
			private readonly IApi _standardApi;
			private readonly MemoryResponseCache _cache;

			public ExampleApi()
			{
				var apiUri = new ApiUri();
				var oauthCredentials = new AppSettingsCredentials();
				_standardApi = new ApiFactory(apiUri, oauthCredentials);

				_cache = new MemoryResponseCache(MemoryCache.Default);
			}

			public IFluentApi<T> Create<T>() where T : class, new()
			{
				return _standardApi.Create<T>()
					.UsingCache(_cache);
			}
		}