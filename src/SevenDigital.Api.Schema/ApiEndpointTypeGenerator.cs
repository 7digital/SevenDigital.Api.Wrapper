using System;
using System.Linq;
using System.Reflection;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema
{
	public class ApiEndpointTypeGenerator : ITypeGenerator
	{
		public Type GenerateType(string endpoint)
		{
			Assembly assembly = Assembly.GetAssembly(GetType());

			Type[] types = assembly.GetTypes();
			Type correctType = null;
			foreach (var type in types)
			{
				ApiEndpointAttribute attribute = type.GetCustomAttributes(true)
					.OfType<ApiEndpointAttribute>()
					.FirstOrDefault();

				if (attribute == null || attribute.EndpointUri != endpoint)
					continue;

				correctType = type;
				break;
			}

			if (correctType == null)
				throw new ArgumentException("Could not find endpoint defined with this name");

			return correctType;
		}
	}
}