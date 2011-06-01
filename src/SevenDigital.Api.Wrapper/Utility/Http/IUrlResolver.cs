using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IUrlResolver{
        string Resolve(Uri endpoint, string method, Dictionary<string,string> headers);
	    void ResolveAsync(Uri endpoint, string method, Dictionary<string, string> headers, Action<string> payload);
	}
}