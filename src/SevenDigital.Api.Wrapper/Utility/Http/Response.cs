using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{	
	public interface IResponse<T>
	{
		Dictionary<string, string> Headers { get; }
		T Body { get; }
	}

	public class Response<T> : IResponse<T>
	{
		public Dictionary<string, string> Headers { get; set; }
		public T Body { get; set; }

		public Response() 
		{
			Headers = new Dictionary<string, string>();
		}
	}
}