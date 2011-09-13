using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class DependencyChecker<T> where T : class
	{
		private static DependencyChecker<T> _instance;
		private static T _dependency;

		private DependencyChecker() { }

		public static DependencyChecker<T> Instance
		{
			get {
				return _instance ?? (_instance = new DependencyChecker<T>());
			}
		}

		public T Dependency
		{
			get
			{
				if (_dependency == null)
					_dependency = CheckIfAssembliesContainOAuthClass();

				return _dependency;
			}
		}


		private static T CheckIfAssembliesContainOAuthClass()
		{
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			var enumerable = new List<Type>();
			foreach (var loadedAssembly in loadedAssemblies)
			{
				try {
					enumerable.AddRange(GetValidTypes(loadedAssembly));
				}catch(Exception) {}
			}
			if (enumerable.Count() < 1)
				throw new MissingDependencyException(
					String.Format(
					"You need an implementation of {0} in your app.", typeof(T).FullName));

			Type firstOrDefault = enumerable.FirstOrDefault();
			return (T)Activator.CreateInstance(firstOrDefault);
		}

		private static IEnumerable<Type> GetValidTypes(Assembly assembly)
		{
			Type type = typeof(T);
			return assembly.GetTypes().Where(x => type.IsAssignableFrom(x) && x != type);
		}
	}
}