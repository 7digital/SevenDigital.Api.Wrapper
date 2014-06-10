using System;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasTagsExtensions
	{
		public static IFluentApi<T> WithTags<T>(this IFluentApi<T> api, params string[] tags) where T : HasTags
		{
			api.WithParameter("tags", String.Join(",",tags));
			return api;
		}
	}
}
