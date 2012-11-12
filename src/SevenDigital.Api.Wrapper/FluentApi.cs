using System;
using System.Collections.Generic;
using System.Linq;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Serialization;

namespace SevenDigital.Api.Wrapper
{
	public class FluentApi<T> : IFluentApi<T> where T : class
	{
		private readonly RequestData _requestData = new RequestData();
		private readonly IRequestCoordinator _requestCoordinator;
		private readonly IResponseParser<T> _parser;

		public FluentApi(IRequestCoordinator requestCoordinator)
		{
			_requestCoordinator = requestCoordinator;

			_parser = new ResponseParser<T>();

			ApiEndpointAttribute attribute = typeof(T).GetCustomAttributes(true)
				.OfType<ApiEndpointAttribute>()
				.FirstOrDefault();

			if (attribute == null)
			{
				throw new ArgumentException(string.Format("The Type {0} cannot be used in this way, it has no ApiEndpointAttribute", typeof(T)));
			}

			_requestData.UriPath = attribute.EndpointUri;


			OAuthSignedAttribute isSigned = typeof(T).GetCustomAttributes(true)
				.OfType<OAuthSignedAttribute>()
				.FirstOrDefault();

			if (isSigned != null)
				_requestData.IsSigned = true;

			RequireSecureAttribute isSecure = typeof(T).GetCustomAttributes(true)
				.OfType<RequireSecureAttribute>()
				.FirstOrDefault();

			if (isSecure != null)
				_requestData.UseHttps = true;

			HttpPostAttribute isHttpPost = typeof(T).GetCustomAttributes(true)
				.OfType<HttpPostAttribute>()
				.FirstOrDefault();

			if (isHttpPost != null)
			{
				_requestData.HttpMethod = "POST";
			}

		}

		public FluentApi(IOAuthCredentials oAuthCredentials, IApiUri apiUri)
			: this(new RequestCoordinator(new GzipHttpClient(), new UrlSigner(), oAuthCredentials, apiUri)) { }

		public FluentApi()
			: this(new RequestCoordinator(new GzipHttpClient(), new UrlSigner(), 
				EssentialDependencyCheck<IOAuthCredentials>.Instance, EssentialDependencyCheck<IApiUri>.Instance)) 
			{ }


		public IFluentApi<T> WithEndpoint(string endpoint)
		{
			_requestData.UriPath = endpoint;
			return this;
		}

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
			try
			{
				var response = _requestCoordinator.HitEndpoint(_requestData);
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