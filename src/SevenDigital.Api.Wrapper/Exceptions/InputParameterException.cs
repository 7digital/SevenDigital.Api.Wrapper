using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class InputParameterException : ApiErrorException
	{
		public InputParameterException()
			: base()
		{
		}

		public InputParameterException(string message, Response response, Error error)
			: base(message, response, error)
		{
		}

		public InputParameterException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected InputParameterException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}