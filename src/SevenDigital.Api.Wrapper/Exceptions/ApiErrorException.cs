using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiErrorException : ApiResponseException
	{
		public ErrorCode ErrorCode { get; private set; }

		protected ApiErrorException(string message, Response response, ErrorCode errorCode)
			: base(message, response)
		{
			ErrorCode = errorCode;
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected ApiErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			ErrorCode = (ErrorCode)info.GetValue("ErrorCode", typeof(ErrorCode));
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			info.AddValue("ErrorCode", ErrorCode);

			base.GetObjectData(info, context);
		}
	}
}
