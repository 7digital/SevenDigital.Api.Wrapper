using System;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class HasTagsExtensions
	{
		public static IApiRequest<T> WithTags<T>(this IApiRequest<T> api, params string[] tags) where T : HasTags
		{
			api.WithParameter("tags", String.Join(",",tags));
			return api;
		}
	}
}
