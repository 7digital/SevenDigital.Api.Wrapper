using System;
using System.Collections.Generic;
using System.Net;
using SevenDigital.Api.Wrapper.AttributeManagement;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Serialization;

namespace SevenDigital.Api.Wrapper
{
	public class FluentApi<T> : IFluentApi<T> where T : class
	{
		private readonly RequestData _requestData;
		private readonly IRequestCoordinator _requestCoordinator;
		private readonly IResponseParser<T> _parser;
		private ICache _cache = new NullCache();
		private bool _checkXmlValidity; 

		public FluentApi(IRequestCoordinator requestCoordinator)
		{
			var attributeValidation = new AttributeRequestDataBuilder<T>();
			_requestData = attributeValidation.BuildRequestData();

			_requestCoordinator = requestCoordinator;

			_parser = new ResponseParser<T>();
		}

		public FluentApi(IOAuthCredentials oAuthCredentials, IApiUri apiUri)
			: this(new RequestCoordinator(new GzipHttpClient(), new UrlSigner(), oAuthCredentials, apiUri)) { }

		public FluentApi()
			: this(new RequestCoordinator(new GzipHttpClient(), new UrlSigner(), 
				EssentialDependencyCheck<IOAuthCredentials>.Instance, EssentialDependencyCheck<IApiUri>.Instance)) 
			{ }

		public IFluentApi<T> UsingClient(IHttpClient httpClient)
		{
			_requestCoordinator.HttpClient = httpClient;
			return this;
		}

		public IFluentApi<T> WithCheckXmlValidity()
		{
			_checkXmlValidity = true;
			return this;
		}

		public virtual IFluentApi<T> WithMethod(string methodName)
		{
			_requestData.HttpMethod = methodName;
			return this;
		}

		public virtual IFluentApi<T> WithCache(ICache cache)
		{
			_cache = cache;
			return this;
		}

		public virtual IFluentApi<T> WithParameter(string parameterName, string parameterValue)
		{
			_requestData.Parameters[parameterName] = parameterValue;
			return this;
		}

		public virtual IFluentApi<T> ClearParameters()
		{
			_requestData.Parameters.Clear();
			return this;
		}

		public virtual IFluentApi<T> ForUser(string token, string secret)
		{
			_requestData.UserToken = token;
			_requestData.TokenSecret = secret;
			return this;
		}

		public virtual IFluentApi<T> ForShop(int shopId)
		{
			WithParameter("shopId", shopId.ToString());
			return this;
		}

		public virtual T Please()
		{
			string cachedResponse;
			var foundInCache = _cache.TryGet(EndpointUrl, out cachedResponse);
			if (foundInCache)
			{
				var responseFromCache = new Response(HttpStatusCode.OK, cachedResponse);
				return _parser.Parse(responseFromCache, false);
			}

			Response response;
			try
			{
				response = _requestCoordinator.HitEndpoint(_requestData);
			}
			catch (WebException webException)
			{
				throw new ApiWebException(webException.Message, EndpointUrl, webException);
			}

			try
			{
				var result = _parser.Parse(response, _checkXmlValidity);

				// set to cache only after it has passed validity checks
				_cache.Set(EndpointUrl, response.Body);
				return result;
			}
			catch (ApiResponseException apiXmlException)
			{
				apiXmlException.Uri = EndpointUrl;
				throw;
			}
		}

		public virtual string EndpointUrl
		{
			get { return _requestCoordinator.ConstructEndpoint(_requestData); }
		}

		public virtual void PleaseAsync(Action<T> callback)
		{
			_requestCoordinator.HitEndpointAsync(_requestData, PleaseAsyncEnd(callback));
		}

		internal Action<Response> PleaseAsyncEnd(Action<T> callback)
		{
			return output =>
			{
				T entity = _parser.Parse(output, _checkXmlValidity);
				callback(entity);
			};
		}

		public IDictionary<string, string> Parameters
		{
			get { return _requestData.Parameters; }
		}
	}
}