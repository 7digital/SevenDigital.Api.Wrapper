using System;
using System.Linq;
using System.Reflection;
using System.Xml;
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

        public FluentApi()
            : this(new EndpointResolver(new HttpGetResolver(), new UrlSigner()))
        {}

        public FluentApi(IEndpointResolver endpointResolver)
        {
            _endpointResolver = endpointResolver;

            ApiEndpointAttribute attribute = typeof(T).GetCustomAttributes(true)
                                                .OfType<ApiEndpointAttribute>()
                                                .FirstOrDefault();
            if (attribute == null)
                throw new ArgumentException(string.Format("The Type {0} cannot be used in this way, it has no ApiEndpointAttribute", typeof(T)));

			_endPointInfo.Uri = attribute.EndpointUri;


			OAuthSignedAttribute isSigned =  typeof(T).GetCustomAttributes(true)
                                                .OfType<OAuthSignedAttribute>()
                                                .FirstOrDefault();

			if (isSigned != null)
				_endPointInfo.IsSigned = true;
        }

    	public IFluentApi<T> WithEndpoint(string endpoint)
        {
            _endPointInfo.Uri = endpoint;
            return this;
        }

        public IFluentApi<T> WithMethod(string methodName)
        {
            _endPointInfo.HttpMethod = methodName;
            return this;
        }

        public IFluentApi<T> WithParameter(string parameterName, string parameterValue)
        {
            _endPointInfo.Parameters.Set(parameterName, parameterValue);
            return this;
        }

        public IFluentApi<T> ForUser(string token, string secret)
        {
            _endPointInfo.UserToken = token;
            _endPointInfo.UserSecret = secret;
            return this;
        }

        public T Please()
        {
            XmlNode output = _endpointResolver.HitEndpoint(_endPointInfo);
            var xmlSerializer = new XmlSerializer<T>();
            return xmlSerializer.DeSerialize(output);
        }
    }
}