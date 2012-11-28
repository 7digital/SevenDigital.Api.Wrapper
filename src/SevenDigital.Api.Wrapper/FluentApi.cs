using System;
using System.Collections.Generic;
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
		private readonly EndpointContext _endpointContext;
		private readonly RequestContext _requestContext;
		private readonly IRequestCoordinator _requestCoordinator;
		private readonly IResponseParser<T> _parser;

		public FluentApi(IRequestCoordinator requestCoordinator)
		{
			var attributeValidation = new AttributeEndpointContextBuilder<T>();
			_endpointContext = attributeValidation.BuildRequestData();
			_requestContext = new RequestContext();
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

		public virtual IFluentApi<T> WithMethod(string methodName)
		{
			_endpointContext.HttpMethod = methodName;
			return this;
		}

		public virtual IFluentApi<T> WithParameter(string parameterName, string parameterValue)
		{
			_requestContext.Parameters[parameterName] = parameterValue;
			return this;
		}

		public virtual IFluentApi<T> ClearParameters()
		{
			_requestContext.Parameters.Clear();
			return this;
		}

		public virtual IFluentApi<T> ForUser(string token, string secret)
		{
			_endpointContext.UserToken = token;
			_endpointContext.TokenSecret = secret;
			return this;
		}

		public virtual IFluentApi<T> ForShop(int shopId)
		{
			WithParameter("shopId", shopId.ToString());
			return this;
		}

		public virtual T Please()
		{
			try
			{
				var response = _requestCoordinator.HitEndpoint(_endpointContext, _requestContext);
				return _parser.Parse(response);
			}
			catch (ApiException apiXmlException)
			{
				apiXmlException.Uri = EndpointUrl;
				throw;
			}
		}

		public virtual string EndpointUrl
		{
			get { return _requestCoordinator.ConstructEndpoint(_endpointContext, _requestContext); }
		}

		public virtual void PleaseAsync(Action<T> callback)
		{
			_requestCoordinator.HitEndpointAsync(_endpointContext, _requestContext, PleaseAsyncEnd(callback));
		}

		internal Action<Response> PleaseAsyncEnd(Action<T> callback)
		{
			return output =>
			{
				T entity = _parser.Parse(output);
				callback(entity);
			};
		}

		public IDictionary<string, string> Parameters
		{
			get { return _requestContext.Parameters; }
		}
	}
}