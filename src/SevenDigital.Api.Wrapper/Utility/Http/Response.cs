using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	[Serializable]
	public class Response : IResponse
	{
		public Dictionary<string, string> Headers { get; set; }
		public string Body { get; set; }

		public Response() 
		{
			Headers = new Dictionary<string, string>();
		}
	}
}