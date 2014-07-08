# Release notes for 7digital API Wrapper version 4.0.0 #

Wrapper v 4.0 introduces a number of breaking changes, and if you are upgrading from earlier versions you should be aware of them and upgrade at a time when you are ready to work through them. The major breaking changes are:

## dotNet 4.5 and async ##
The minimum .Net version supported is now .Net 4.5. 

`fluentApi<T>.Please()` now returns `Task<T>` not `T`. This is designed for use with [the `async` and `await` keywords](http://msdn.microsoft.com/en-gb/library/hh191443.aspx).

All example and test code has been updated to match.

## Schema namespaces ##

The namespaces and class names of the schema have been changed to be more consistent. In general, the namespace is the plural, e.g. “`Baskets`” and the main class in it is singular e.g. “`Basket`”

## Instancing ##

The global singleton `Api<T>.Create` [is still present](https://github.com/7digital/SevenDigital.Api.Wrapper/blob/master/src/SevenDigital.Api.Wrapper/StaticApi.cs), but is not recommended for anything beyond the simplest use.

We recommend using [the new `IApi` interface](https://github.com/7digital/SevenDigital.Api.Wrapper/blob/master/src/SevenDigital.Api.Wrapper/Api.cs), using the default implementation or a customised one. This has the following advantages:  
- It is a point to specify your credentials and url classes to avoid the overhead of scanning for them.  
- It gives you a point to cofnigure the Api for all requests (e.g. set a partner ids on all requests or set the cache.)  
- If you are using an IoC container, `IApi` is suitable for registering with  it. It can be registered as a singleton. Because it is an interface with a generic Create<T>() method rather than a generic class, you can inject it once and use it create multiple Api requests of different types.   
- You can use it to create Api multiple requests of the same type, and thus avoid common threading and state issues. We aim to have the calling code do the right thing by default here, and they often did not in the past. Requests should not be re-used or shared, as state is retained within the request. However rhe `IApi `instance that creates them can be shared and long-lived.

The example program and some of the tests have been updated to match.

## Response Caching ##

The [`IResponseCache` cache](https://github.com/7digital/SevenDigital.Api.Wrapper/blob/master/src/SevenDigital.Api.Wrapper/Responses/IResponseCache.cs) now stores objects of the unwrapped type `T` not of type `Response`. If you are supplying an `IResponseCache` implementation,  be aware that data put into cache by previous versions of the wrapper should not be retrieved from cache and used by this version of the wrapper. We recommend that you do one or more of the following to avoid retrieving invalid items:  
1. Flush your cache when deploying the new version of the wrapper  
2. Use a different cache key with this version of the wrapper  
3. Check that the retrieved object type matches, and if not, act as if it was not found.  
 
## RequestData Oauth Fields ##

On [the `RequestData` class](https://github.com/7digital/SevenDigital.Api.Wrapper/blob/master/src/SevenDigital.Api.Wrapper/Requests/RequestData.cs), `UserToken` has been renamed to `OauthToken` and `TokenSecret` / `UserSecret` has been renamed to `OauthTokenSecret` as these names are in line with [standard OAuth naming](http://oauth.net/core/1.0/#auth_step3).

## Advanced Usage ##

More ways to get data out. We have added the `Response()` and `ResponseAs<TR>()` methods to [the `IFluentApi`](https://github.com/7digital/SevenDigital.Api.Wrapper/blob/master/src/SevenDigital.Api.Wrapper/IFluentApi.cs), in addition to the `Please()` method.

`Response()` will issue the request and return the `Response` object as a `Task<Response>`. You can inspect the status code, response headers and response body text. You are then responsible for deserialising this as you see fit.

`ResponseAs<TR>()` will attempt to deserialise the response as the the supplied type `TR` and returns `Task<TR>`. The type `TR` does not need to be the same as the endpoint class `T`.


----------------------------