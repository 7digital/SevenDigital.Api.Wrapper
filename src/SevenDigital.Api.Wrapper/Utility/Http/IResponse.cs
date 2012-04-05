using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IResponse
	{
		Dictionary<string, string> Headers { get; }
		string Body { get; }
	}
}