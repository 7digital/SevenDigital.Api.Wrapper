using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IRequestDispatcher {
		string Dispatch(string endpoint, Dictionary<string, string> headers);
		Response<string> FullDispatch(string endpoint, Dictionary<string, string> headers);
		void DispatchAsync(string endpoint, Dictionary<string, string> headers, Action<string> payload);
	}
}