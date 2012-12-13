using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class MissingDependencyException : Exception
	{
		public MissingDependencyException(Type dependency)
			: base(String.Format("You need a class that implements {0} in your application. ", dependency.FullName))
		{
		}

		protected MissingDependencyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

	}
}