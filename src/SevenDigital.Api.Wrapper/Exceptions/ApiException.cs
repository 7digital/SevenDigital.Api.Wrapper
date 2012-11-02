using System;
using System.Net;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiException : Exception
	{
		public string Uri { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public string ResponseBody { get; set; }

		protected ApiException()
		{
		}

		protected ApiException(string message)
			: base(message)
		{
		}

		protected ApiException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}