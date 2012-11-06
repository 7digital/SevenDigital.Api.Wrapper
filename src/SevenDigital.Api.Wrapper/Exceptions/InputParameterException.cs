using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class InputParameterException : ApiErrorException
	{
		public InputParameterException()
			: base()
		{
		}

		public InputParameterException(string message)
			: base(message)
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