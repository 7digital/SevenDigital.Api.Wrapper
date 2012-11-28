
using System.Collections.Generic;
using SevenDigital.Api.Wrapper.AttributeManagement;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper
{
	public class FluentApi<T> : IFluentApi<T> where T : class
	{
		private readonly EndpointContext _endpointContext;
		private readonly RequestContext _requestContext;
		private readonly IRequestCoordinator _requestCoordinator;

		public FluentApi(IRequestCoordinator requestCoordinator)
		{
			var attributeValidation = new AttributeEndpointContextBuilder<T>();
			_endpointContext = attributeValidation.BuildRequestData();
			_requestContext = new RequestContext();
			_requestCoordinator = requestCoordinator;
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

		public virtual string EndpointUrl
		{
			get { return _requestCoordinator.ConstructEndpoint(_endpointContext, _requestContext); }
		}

		public Request<T> Seal()
		{
			return new Request<T>(_requestCoordinator, _endpointContext, _requestContext);
		}

		public IDictionary<string, string> Parameters
		{
			get { return _requestContext.Parameters; }
		}
	}
}