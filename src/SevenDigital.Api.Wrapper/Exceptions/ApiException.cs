using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

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

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected ApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Uri = info.GetString("Uri");
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			info.AddValue("Uri", Uri);

			base.GetObjectData(info, context);
		}
	}
}