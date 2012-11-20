using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Http
{
	public class RequestContext
	{
		public IDictionary<string,string> Parameters { get; set; }

		public IDictionary<string,string> Headers { get; set; }

		public RequestContext()
		{
			Parameters = new Dictionary<string,string>();
			Headers = new Dictionary<string,string>();
		}
	}
}