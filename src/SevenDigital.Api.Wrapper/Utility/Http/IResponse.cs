using System.Collections.Generic;
using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IResponse
	{
		Dictionary<string, string> Headers { get; }
		string Body { get; }
		HttpStatusCode StatusCode { get; }
	}
}