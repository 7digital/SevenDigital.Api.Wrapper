using System;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class MissingDependencyException : Exception
	{
		public MissingDependencyException(Type dependency)
			: base(String.Format("You need a class that implements {0} in your application. ", dependency.FullName))
		{ }
	}
}