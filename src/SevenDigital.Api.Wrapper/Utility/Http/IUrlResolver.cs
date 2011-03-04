using System;
using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IUrlResolver{
		string Resolve(Uri endpoint, string method, WebHeaderCollection headers);
	}
}