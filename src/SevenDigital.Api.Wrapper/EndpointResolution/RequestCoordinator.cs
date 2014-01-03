using System;
using System.Collections.Generic;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class RequestCoordinator : IRequestCoordinator
	{
		private readonly IEnumerable<RequestHandler> _requestHandlers;

		public IHttpClient HttpClient { get; set; }

		public RequestCoordinator(IHttpClient httpClient, IEnumerable<RequestHandler> requestHandlers)
		{
			HttpClient = httpClient;
			_requestHandlers = requestHandlers;
		}

		public string GetDebugUri(RequestData requestData)
		{
			var requestHandler = FindRequestHandler(requestData.HttpMethod);
			return requestHandler.GetDebugUri(requestData);
		}

		private RequestHandler FindRequestHandler(string httpMethod)
		{
			var upperHttpMethodName = httpMethod.ToUpperInvariant();
			foreach (var requestHandler in _requestHandlers)
			{
				if (requestHandler.HandlesMethod(upperHttpMethodName))
				{
					return requestHandler;
				}
			}

			string errorMessage = string.Format("No RequestHandler supplied for method '{0}'", upperHttpMethodName);
			throw new NotImplementedException(errorMessage);
		}

		public virtual Response HitEndpoint(RequestData requestData)
		{
			var requestHandler = FindRequestHandler(requestData.HttpMethod);
			requestHandler.HttpClient = HttpClient;
			return requestHandler.HitEndpoint(requestData);
		}

		public virtual void HitEndpointAsync(RequestData requestData, Action<Response> callback)
		{
			var requestHandler = FindRequestHandler(requestData.HttpMethod);
			requestHandler.HttpClient = HttpClient;
			requestHandler.HitEndpointAsync(requestData, callback);
		}
	}
}