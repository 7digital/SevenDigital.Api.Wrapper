using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IRequest
	{
		string Url { get; }
		string Body { get; }
		Dictionary<string, string> Headers { get; } 
	}
}