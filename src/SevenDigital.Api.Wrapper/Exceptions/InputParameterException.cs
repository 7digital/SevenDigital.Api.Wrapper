using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class InputParameterException : ApiErrorException
	{
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