using System;
using System.Net;
using System.Text;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IWebClientWrapper : IDisposable {
		string UploadString(string address, string method, string data);
		Encoding Encoding { get; set; }
		WebHeaderCollection Headers { get; set; }
	}
}