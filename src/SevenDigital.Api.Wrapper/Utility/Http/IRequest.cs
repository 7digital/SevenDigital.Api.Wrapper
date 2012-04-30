using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IRequest
	{
		string Url { get; }
		string Body { get; }
		IDictionary<string, string> Headers { get; } 
		IDictionary<string, string> Parameters { get; } 
	}
}