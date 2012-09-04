using System;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public interface IRequestCoordinator
	{
		Response HitEndpoint(EndPointInfo endPointInfo);
		void HitEndpointAsync(EndPointInfo endPointInfo, Action<Response> callback);

		string ConstructEndpoint(EndPointInfo endPointInfo);
		IHttpClient HttpClient { get; set; }
	}
}