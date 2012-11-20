using System;
using System.Collections.Generic;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper
{
	public class ApiRequest<T> : IApiRequest<T>
	{
		private readonly IFluentApi<T> _fluentApi;
		public RequestContext RequestContext { get; private set; }

		public ApiRequest(IFluentApi<T> fluentApi)
		{
			_fluentApi = fluentApi;
			RequestContext = new RequestContext();
		}

		public IApiRequest<T> WithParameter(string key, string value)
		{
			RequestContext.Parameters.Add(key, value);
			return this;
		}

		public IApiRequest<T> ClearParameters()
		{
			RequestContext.Parameters = new Dictionary<string, string>();
			return this;
		}

		public virtual IApiRequest<T> ForShop(int shopId)
		{
			WithParameter("shopId", shopId.ToString());
			return this;
		}

		public T Please()
		{
			return _fluentApi.Please(RequestContext);
		}

		public void PleaseAsync(Action<T> callback)
		{
			_fluentApi.PleaseAsync(RequestContext, callback);
		}
	}
}