using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class HasReleaseTypeParameterExtensions
	{
		public static IFluentApi<T> ForReleaseType<T>(this IFluentApi<T> api, ReleaseType releaseType) where T : HasReleaseTypeParameter
		{
			return api.ForReleaseType(releaseType.ToString());
		}

		public static IFluentApi<T> ForReleaseType<T>(this IFluentApi<T> api, string releaseType) where T : HasReleaseTypeParameter
		{
			api.WithParameter("type", releaseType.ToString());
			return api;
		}
	}
}