using System;
using System.Net;
using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class ApiXmlException : Exception
	{
		public string Uri { get; set; }
		public Error Error { get; private set; }
		public HttpStatusCode StatusCode { get; private set; }

		public ApiXmlException(string message)
			: base(message)
		{
		}

		public ApiXmlException(string message, Error error)
			: base(message)
		{
			Error = error;
		}

		public ApiXmlException(string message, HttpStatusCode statusCode, Exception innerException)
			: base(message, innerException)
		{
			StatusCode = statusCode;
			Error = new Error
				{
					ErrorMessage = "Unable to deserialize XML"
				};
		}

		public ApiXmlException(string message, HttpStatusCode statusCode)
			: this(message)
		{
			StatusCode = statusCode;
			Error = new Error
			{
				ErrorMessage = "Unable to deserialize XML"
			};
		}

		public ApiXmlException(string message, HttpStatusCode statusCode, Error error)
			: this(message)
		{
			StatusCode = statusCode;
			Error = error;
		}
	}
}