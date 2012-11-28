using System;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Serialization;

namespace SevenDigital.Api.Wrapper
{
	public class Request<T> where T: class
	{
		private readonly IRequestCoordinator _requestCoordinator;
		private readonly EndpointContext _endpointContext;
		private readonly RequestContext _requestContext;
		private readonly IResponseParser<T> _parser;

		public Request(IRequestCoordinator requestCoordinator, EndpointContext endpointContext, RequestContext requestContext)
		{
			_requestCoordinator = requestCoordinator;
			_endpointContext = new EndpointContext(endpointContext);
			_requestContext = new RequestContext(requestContext);
			_parser = new ResponseParser<T>();
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
				apiXmlException.Uri = _endpointContext.Url;
				throw;
			}
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
	}
}
