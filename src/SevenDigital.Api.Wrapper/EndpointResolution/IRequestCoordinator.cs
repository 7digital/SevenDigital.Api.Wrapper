using System;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public interface IRequestCoordinator
	{
		Response HitEndpoint(EndpointContext endpointContext, RequestContext requestContext);
		void HitEndpointAsync(EndpointContext endpointContext, RequestContext requestContext, Action<Response> callback);

		string ConstructEndpoint(EndpointContext endpointContext, RequestContext requestContext);
		IHttpClient HttpClient { get; set; }
	}
}