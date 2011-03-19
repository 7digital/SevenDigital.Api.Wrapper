using System.Xml;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public interface IEndpointResolver
	{
		XmlNode HitEndpoint(EndPointInfo endPointInfo);
	}
}