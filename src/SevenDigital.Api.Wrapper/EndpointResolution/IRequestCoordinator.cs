using System;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public interface IRequestCoordinator
	{
		Response HitEndpoint(RequestData requestData);

		string ConstructEndpoint(RequestData requestData);
		IHttpClient HttpClient { get; set; }
	}
}