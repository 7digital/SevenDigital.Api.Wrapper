using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Requests
{
	public class ApiRequest
	{
		public string AbsoluteUrl { get; set; }
		public IDictionary<string, string> Parameters { get; set; }
	}
}