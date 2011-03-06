using System.Collections.Specialized;
using System.Xml;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public interface IEndpointResolver
	{
		XmlNode HitEndpoint(string endPoint, string methodName, NameValueCollection querystring);
	}
}