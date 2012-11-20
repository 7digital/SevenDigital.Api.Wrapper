using System;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper
{
	public interface IApiRequest<T>
	{
		RequestContext RequestContext { get; }

		IApiRequest<T> WithParameter(string key, string value);
		IApiRequest<T> ClearParameters();

		T Please();
		void PleaseAsync(Action<T> callback);
		IApiRequest<T> ForShop(int shopId);
	}
}