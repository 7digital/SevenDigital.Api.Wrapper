using System.Threading.Tasks;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Http
{
	public interface IHttpClient
	{
		Task<Response> Send(Request request);
	}
}