using System;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public class MissingDependencyException : Exception
	{
		public MissingDependencyException(string message)
			: base(message)
		{ }
	}
}