using System;
using System.Collections.Generic;
using System.Linq;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Wrapper.Utility.Http;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper
{
	public class FluentApi<T> : IFluentApi<T> where T : class
	{
		private readonly EndPointInfo _endPointInfo = new EndPointInfo();
		private readonly IEndpointResolver _endpointResolver;


		public FluentApi(IEndpointResolver endpointResolver)
		{
			_endpointResolver = endpointResolver;

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

		public FluentApi()
			: this(new EndpointResolver(new HttpGetResolver(), new UrlSigner(), DependencyChecker<IOAuthCredentials>.Instance.Dependency, DependencyChecker<IApiUri>.Instance.Dependency)) { }


		public IFluentApi<T> WithEndpoint(string endpoint)
		{
			_endPointInfo.Uri = endpoint;
			return this;
		}

		public virtual IFluentApi<T> WithMethod(string methodName)
		{
			_endPointInfo.HttpMethod = methodName;
			return this;
		}

		public virtual IFluentApi<T> WithParameter(string parameterName, string parameterValue)
		{
			_endPointInfo.Parameters[parameterName] = parameterValue;
			return this;
		}

		public virtual IFluentApi<T> ForUser(string token, string secret)
		{
			_endPointInfo.UserToken = token;
			_endPointInfo.UserSecret = secret;
			return this;
		}

		public virtual T Please()
		{
			var output = _endpointResolver.HitEndpoint(_endPointInfo);
			var xmlSerializer = new ApiXmlDeSerializer<T>(new ApiResourceDeSerializer<T>());
			return xmlSerializer.DeSerialize(output);
		}

		public virtual void PleaseAsync(Action<T> callback)
		{
			_endpointResolver.HitEndpointAsync(_endPointInfo, PleaseAsyncEnd(callback));
		}

		internal static Action<string> PleaseAsyncEnd(Action<T> callback)
		{
			return output =>
					   {
						   var xmlSerializer = new ApiXmlDeSerializer<T>(new ApiResourceDeSerializer<T>());
						   T entity = xmlSerializer.DeSerialize(output);

						   callback(entity);
					   };
		}

		public IDictionary<string,string> Parameters { get { return _endPointInfo.Parameters; } }
	}
}