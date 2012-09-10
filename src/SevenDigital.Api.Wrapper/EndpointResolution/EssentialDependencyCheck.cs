using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class EssentialDependencyCheck<TClass> where TClass : class
	{
		private static TClass _currentTClassInstance;
		
		public static TClass Instance
		{
			get 
			{
				return _currentTClassInstance ??
					(_currentTClassInstance = FindTClassInLocalAssemblies());
			}
		}

		private static TClass FindTClassInLocalAssemblies() 
		{
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			var allTypesFound = FindAllTypesThatImplementTClass(loadedAssemblies);

			if (allTypesFound.Count() < 1)
			{
				throw new MissingDependencyException(typeof(TClass));
			}

			Type firstTClassInstanceFound = allTypesFound.FirstOrDefault();
			return (TClass)Activator.CreateInstance(firstTClassInstanceFound);
		}

		private static IList<Type> FindAllTypesThatImplementTClass(IEnumerable<Assembly> loadedAssemblies) 
		{
			var allTypesFound = new List<Type>();
			foreach (var loadedAssembly in loadedAssemblies) 
			{
				try 
				{
					IEnumerable<Type> typesOfTClass = GetConcreteTypesThatImplementTClass(loadedAssembly);
					allTypesFound.AddRange(typesOfTClass);
				}
				catch (Exception) 
				{}
			}
			return allTypesFound;
		}

		private static IEnumerable<Type> GetConcreteTypesThatImplementTClass(Assembly assembly)
		{
			Type type = typeof (TClass);
			return assembly.GetTypes().Where(x => type.IsAssignableFrom(x) && x != type);
		}
	}
}