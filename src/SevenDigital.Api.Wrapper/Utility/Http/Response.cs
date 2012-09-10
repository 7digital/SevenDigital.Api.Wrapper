using System;
using System.Collections.Generic;
using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	[Serializable]
	public class Response
	{
		public Dictionary<string, string> Headers { get; set; }
		public string Body { get; set; }

		public HttpStatusCode StatusCode { get; set; }

		public Response() 
		{
			Headers = new Dictionary<string, string>();
		}
	}
}