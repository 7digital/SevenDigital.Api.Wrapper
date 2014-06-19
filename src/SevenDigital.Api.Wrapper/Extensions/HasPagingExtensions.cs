using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPagingExtensions
	{
		public static IFluentApi<T> WithPageNumber<T>(this IFluentApi<T> api, int pageNumber) where T : IHasPaging
		{
			api.WithParameter("page", pageNumber);
			return api;
		}

		public static IFluentApi<T> WithPageSize<T>(this IFluentApi<T> api, int pageSize) where T : IHasPaging
		{
			api.WithParameter("pageSize", pageSize);
			return api;
		}
	}
}