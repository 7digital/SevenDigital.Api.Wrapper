using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper
{
	public static class HasPagingExtensions
	{
		public static IApiRequest<T> WithPageNumber<T>(this IApiRequest<T> api, int pageNumber) where T : IHasPaging
		{
			api.WithParameter("page", pageNumber.ToString());
			return api;
		}

		public static IApiRequest<T> WithPageSize<T>(this IApiRequest<T> api, int pageSize) where T : IHasPaging
		{
			api.WithParameter("pageSize", pageSize.ToString());
			return api;
		}
	}
}