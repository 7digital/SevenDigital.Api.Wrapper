using System;
using System.Collections.Generic;
using System.Linq;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper {
	public class FluentApi<T> : IFluentApi<T> where T : class {
		private readonly EndPointInfo _endPointInfo = new EndPointInfo();
		private readonly IEndpointResolver _endpointResolver;
		private readonly IDeSerializer<T> _deserializer;
		
		public FluentApi(IEndpointResolver endpointResolver) {
			_endpointResolver = endpointResolver;

			_deserializer = new ApiXmlDeSerializer<T>(new ApiResourceDeSerializer<T>(), new XmlErrorHandler());


			ApiEndpointAttribute attribute = typeof(T).GetCustomAttributes(true)
												.OfType<ApiEndpointAttribute>()
												.FirstOrDefault();
			if (attribute == null)
				throw new ArgumentException(string.Format("The Type {0} cannot be used in this way, it has no ApiEndpointAttribute", typeof(T)));

			_endPointInfo.Uri = attribute.EndpointUri;


			OAuthSignedAttribute isSigned = typeof(T).GetCustomAttributes(true)
												.OfType<OAuthSignedAttribute>()
												.FirstOrDefault();

			if (isSigned != null)
				_endPointInfo.IsSigned = true;

		}

		public FluentApi(IOAuthCredentials oAuthCredentials, IApiUri apiUri)
			: this(new EndpointResolver(new HttpGetResolver(), new UrlSigner(), oAuthCredentials, apiUri)) { }

		public FluentApi()
			: this(new EndpointResolver(new HttpGetResolver(), new UrlSigner(), EssentialDependencyCheck<IOAuthCredentials>.Instance, EssentialDependencyCheck<IApiUri>.Instance)) { }


		public IFluentApi<T> WithEndpoint(string endpoint) {
			_endPointInfo.Uri = endpoint;
			return this;
		}

		public virtual IFluentApi<T> WithMethod(string methodName) {
			_endPointInfo.HttpMethod = methodName;
			return this;
		}

		public virtual IFluentApi<T> WithParameter(string parameterName, string parameterValue) {
			_endPointInfo.Parameters[parameterName] = parameterValue;
			return this;
		}

		public virtual IFluentApi<T> WithHeader(string headerName, string headerValue)
		{
			_endPointInfo.Headers[headerName] = headerValue;
			return this;
		}

		public virtual IFluentApi<T> ClearParameters() {
			_endPointInfo.Parameters.Clear();
			return this;
		}

		public virtual IFluentApi<T> ForUser(string token, string secret) {
			_endPointInfo.UserToken = token;
			_endPointInfo.UserSecret = secret;
			return this;
		}

		public virtual IFluentApi<T> ForShop(int shopId) {
			WithParameter("shopId", shopId.ToString());
			return this;
		}

		public virtual T Please() {
			try {
				var output = _endpointResolver.HitEndpoint(_endPointInfo);
				return _deserializer.DeSerialize(output);
			} catch (ApiXmlException apiXmlException) {
				apiXmlException.Uri = _endPointInfo.Uri;
				throw;
			}
		}

		public virtual void PleaseAsync(Action<T> callback) {
			_endpointResolver.HitEndpointAsync(_endPointInfo, PleaseAsyncEnd(callback));
		}

		public string GetCurrentUri() {
			return _endpointResolver.ConstructEndpoint(_endPointInfo);
		}

		internal Action<string> PleaseAsyncEnd(Action<T> callback) {
			return output => {
				T entity = _deserializer.DeSerialize(output);
				callback(entity);
			};
		}

		public IDictionary<string, string> Parameters { get { return _endPointInfo.Parameters; } }
	}
}