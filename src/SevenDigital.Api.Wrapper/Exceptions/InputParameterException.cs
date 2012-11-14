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

		public InputParameterException(string message, ErrorCode errorCode)
			: base(message, errorCode)
		{
		}

		public InputParameterException(string message, Response response, ErrorCode errorCode)
			: base(message, response, errorCode)
		{
		}

		protected InputParameterException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}