using System;
using System.Collections.Generic;
using System.Reflection;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class AppDomainAssemblyResolver
	{
		public IEnumerable<Assembly> GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}
	}
}
