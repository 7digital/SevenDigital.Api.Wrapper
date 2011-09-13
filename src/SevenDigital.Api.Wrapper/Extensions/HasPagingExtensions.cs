using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPagingExtensions
	{
		public static IFluentApi<T> WithPageNumber<T>(this IFluentApi<T> api, int pageNumber) where T : HasPaging
		{
			api.WithParameter("page", pageNumber.ToString());
			return api;
		}

		public static IFluentApi<T> WithPageSize<T>(this IFluentApi<T> api, int pageSize) where T : HasPaging
		{
			api.WithParameter("pageSize", pageSize.ToString());
			return api;
		}
	}
}