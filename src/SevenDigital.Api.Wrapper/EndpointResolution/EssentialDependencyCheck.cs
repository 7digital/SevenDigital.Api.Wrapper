using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class EssentialDependencyCheck<TClass> where TClass : class
	{
		private static TClass _dependency;
		
		public static TClass Instance
		{
			get {
				return _dependency ?? (_dependency = FindTClassInLocalAssemblies());
			}
		}

		private static TClass FindTClassInLocalAssemblies() {

			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			var enumerable = new List<Type>();
			foreach (var loadedAssembly in loadedAssemblies)
			{
				try {
					IEnumerable<Type> validTypes = GetValidTypes(loadedAssembly);
					enumerable.AddRange(validTypes);
				}
				catch(Exception) {}
			}

			if (enumerable.Count() < 1)
				throw new MissingDependencyException(typeof(TClass));

			Type firstOrDefault = enumerable.FirstOrDefault();
			return (TClass)Activator.CreateInstance(firstOrDefault);
		}

		private static IEnumerable<Type> GetValidTypes(Assembly assembly)
		{
			Type type = typeof (TClass);
			return assembly.GetTypes().Where(x => type.IsAssignableFrom(x) && x != type);
		}
	}
}