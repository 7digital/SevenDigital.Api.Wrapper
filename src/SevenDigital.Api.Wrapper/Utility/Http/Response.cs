using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public class Response<T> {
		public Dictionary<string, string> Headers { get; set; }
		public T Body { get; set; }

		public Response() {
			Headers = new Dictionary<string, string>();
		}
	}
}