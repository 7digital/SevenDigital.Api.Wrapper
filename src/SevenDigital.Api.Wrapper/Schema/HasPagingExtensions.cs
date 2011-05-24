using SevenDigital.Api.Schema;

namespace SevenDigital.Api.Wrapper.Schema
{
	public static class HasPagingExtensions
	{
		public static IFluentApi<HasPaging> WithPageNumber(this IFluentApi<HasPaging> api, int pageNumber)
		{
			api.WithParameter("page", pageNumber.ToString());
			return api;
		}

		public static IFluentApi<HasPaging> WithPageSize(this IFluentApi<HasPaging> api, int pageSize)
		{
			api.WithParameter("pageSize", pageSize.ToString());
			return api;
		} 
	}
}