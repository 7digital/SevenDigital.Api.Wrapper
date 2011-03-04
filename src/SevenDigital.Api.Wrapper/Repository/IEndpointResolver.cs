using System.Collections.Specialized;
using System.Xml;

namespace SevenDigital.Api.Wrapper.Repository
{
	public interface IEndpointResolver
	{
		XmlNode HitEndpoint(string endPoint, string methodName, NameValueCollection querystring);
	}
}