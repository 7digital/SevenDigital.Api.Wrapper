using System;
using System.Collections.Generic;
using System.Net;
using SevenDigital.Api.Wrapper.AttributeManagement;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
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
		private IResponseCache _responseCache = new NullResponseCache();

		public FluentApi(IRequestCoordinator requestCoordinator)
		{
			var attributeValidation = new AttributeRequestDataBuilder<T>();
			_requestData = attributeValidation.BuildRequestData();

			_requestCoordinator = requestCoordinator;

			_parser = new ResponseParser<T>();
		}

		public FluentApi(IOAuthCredentials oAuthCredentials, IApiUri apiUri)
			: this(new RequestCoordinator(new HttpClientMediator(), new AllRequestHandler(apiUri, oAuthCredentials))) 
			{}

		public FluentApi()
			: this(new RequestCoordinator(new HttpClientMediator(), new AllRequestHandler(EssentialDependencyCheck<IApiUri>.Instance, EssentialDependencyCheck<IOAuthCredentials>.Instance))) 
			{}

		public IFluentApi<T> UsingClient(IHttpClient httpClient)
		{
			if (httpClient == null)
			{
				throw new ArgumentNullException("httpClient");
			}

			_requestCoordinator.HttpClient = httpClient;
			return this;
		}

		public IFluentApi<T> UsingCache(IResponseCache responseCache)
		{
			if (responseCache == null)
			{
				throw new ArgumentNullException("responseCache");
			}

			_responseCache = responseCache;
			return this;
		}

		public virtual IFluentApi<T> WithMethod(string methodName)
		{
			_requestData.HttpMethod =  HttpMethodHelpers.Parse(methodName);
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
			WithParameter("shopId", shopId.ToString());
			return this;
		}

		public virtual T Please()
		{
			Response response;

			bool foundInCache = _responseCache.TryGet(_requestData, out response);
			if (! foundInCache)
			{
				try
				{
					response = _requestCoordinator.HitEndpoint(_requestData);
				}
				catch (WebException webException)
				{
					throw new ApiWebException(webException.Message, EndpointUrl, webException);
				}
			}

			try
			{
				var result = _parser.Parse(response);

				// set to cache only after all validation and parsing has suceeded
				if (!foundInCache)
				{
					_responseCache.Set(_requestData, response);
				}
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

		public IDictionary<string, string> Parameters
		{
			get { return _requestData.Parameters; }
		}
	}
}