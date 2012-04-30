using System;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public interface IRequestCoordinator
	{
		string HitEndpoint(EndPointInfo endPointInfo);
		void HitEndpointAsync(EndPointInfo endPointInfo, Action<string> callback);

		string ConstructEndpoint(EndPointInfo endPointInfo);
		IHttpClient HttpClient { get; set; }
	}
}