using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class CredentialChecker
	{
		private static CredentialChecker _instance;
		private static IOAuthCredentials _credentials;
		
		private CredentialChecker() {}

		public static CredentialChecker Instance {
			get
			{

				if (_instance == null)
					_instance = new CredentialChecker();

				return _instance;
			}
		}

		public IOAuthCredentials Credentials {
			get
			{
				if (_credentials == null)
					_credentials = CheckIfAssembliesContainOAuthClass();

				return _credentials;
			}
		}


		private static IOAuthCredentials CheckIfAssembliesContainOAuthClass() {
           var assemblies = new AppDomainAssemblyResolver().GetAssemblies();


			var enumerable = new List<Type>();
			foreach (var loadedAssembly in assemblies) {
				enumerable.AddRange(GetValidTypes(loadedAssembly));
			}
			if (enumerable.Count() < 1)
				throw new MissingOauthCredentialsException();

			Type firstOrDefault = enumerable.FirstOrDefault();
			return (IOAuthCredentials)Activator.CreateInstance(firstOrDefault);
		}

	    private static IEnumerable<Type> GetValidTypes(Assembly assembly) {
			Type type = typeof(IOAuthCredentials);
			return assembly.GetTypes().Where(x => type.IsAssignableFrom(x) && x != type);
		}
	}
}