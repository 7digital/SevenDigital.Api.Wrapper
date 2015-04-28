using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Wrapper
{
	public static class HasLockerSortExtensions
	{
		public static IFluentApi<T> Sort<T>(this IFluentApi<T> api, LockerSortColumn sortBy, SortOrder sortOrder) where T : HasLockerSort
		{
			var sortConcatenation = sortBy.GetDescription() + " " + sortOrder.GetDescription();
			api.WithParameter("sort", sortConcatenation);
			return api;
		}
	}
}