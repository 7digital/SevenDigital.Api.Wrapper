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

		public virtual IFluentApi<T> WithMethod(string methodName)
		{
			_requestData.HttpMethod = methodName;
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
				return _parser.Parse(response);
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
				T entity = _parser.Parse(output);
				callback(entity);
			};
		}

		public IDictionary<string, string> Parameters
		{
			get { return _requestData.Parameters; }
		}
	}
}