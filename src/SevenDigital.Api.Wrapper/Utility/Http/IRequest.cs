using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IRequest
	{
		string Url { get; }
		Dictionary<string, string> Headers { get; } 
	}
}