using System;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper
{
	// [AD] DO NOT PUT THE OUR BACK IN, NOT SUPPORTED IN WINDOWS PHONE
	// ReSharper disable TypeParameterCanBeVariant
	public interface IFluentApi<T> : IApiEndpoint
	// ReSharper restore TypeParameterCanBeVariant
	{
		IApiRequest<T> MakeRequest();
		IFluentApi<T> ForUser(string token, string secret);
		IFluentApi<T> UsingClient(IHttpClient httpClient);

		T Please(RequestContext requestContext);
		void PleaseAsync(RequestContext requestContext, Action<T> callback);
	}
}
