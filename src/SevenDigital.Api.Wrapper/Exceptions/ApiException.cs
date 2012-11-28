using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiException : Exception
	{
		public string Uri { get; internal set; }

		protected ApiException (string msg, string uri, Exception innerException)
			: base (msg, innerException)
		{
			Uri = uri;
		}

		protected ApiException(string msg) : base(msg)
		{
		}

		protected ApiException(string msg, Exception innerException)
			: base(msg, innerException)
		{
		}

		protected ApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}