using System.Xml;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public interface IEndpointResolver
	{
		string HitEndpoint(EndPointInfo endPointInfo);
	}
}