using System;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public interface IRequestCoordinator
	{
		IResponse HitEndpoint(EndPointInfo endPointInfo);
		void HitEndpointAsync(EndPointInfo endPointInfo, Action<IResponse> callback);

		string ConstructEndpoint(EndPointInfo endPointInfo);
		IHttpClient HttpClient { get; set; }
	}
}