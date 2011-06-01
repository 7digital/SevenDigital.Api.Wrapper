using System;
using System.Xml;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public interface IEndpointResolver
	{
		string HitEndpoint(EndPointInfo endPointInfo);
	    void HitEndpointAsync(EndPointInfo endPointInfo, Action<string> payload);
	}
}