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

		public string ConstructEndpoint(RequestData requestData)
		{
			return ConstructBuilder(requestData).ConstructEndpoint(requestData);
		}

		private RequestHandler ConstructBuilder(RequestData requestData)
		{
			var upperInvariant = requestData.HttpMethod.ToUpperInvariant();
			foreach (var requestHandler in _requestHandlers)
			{
				if (requestHandler.HandlesMethod(upperInvariant))
				{
					return requestHandler;
				}
			}
			throw new NotImplementedException("No RequestHandlers supplied that can deal with this method");
		}

		public virtual Response HitEndpoint(RequestData requestData)
		{
			var builder = ConstructBuilder(requestData);
			builder.HttpClient = HttpClient;
			return builder.HitEndpoint(requestData);
		}

		public virtual void HitEndpointAsync(RequestData requestData, Action<Response> callback)
		{
			var builder = ConstructBuilder(requestData);
			builder.HttpClient = HttpClient;
			builder.HitEndpointAsync(requestData, callback);
		}
	}
}