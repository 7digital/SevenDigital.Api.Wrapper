using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Http
{
#pragma warning disable 1998
	internal class TestingHttpClientHandler : HttpClientHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			return new HttpResponseMessage
			{
				Content = new StringContent("ResponseContent")
			};
		}
	}
#pragma warning restore 1998

	[TestFixture]
	public class HttpClientMediatorTests
	{
		[Test]
		public async Task Http_client_handler_is_used_if_provided()
		{
			var handler = new TestingHttpClientHandler();
			var factory = A.Fake<IHttpClientHandlerFactory>();
			factory.CallsTo(f => f.CreateHandler()).Returns(handler);
			var mediator = new HttpClientMediator(factory);

			var request = new Request(
				HttpMethod.Get,
				"http://localhost",
				new Dictionary<string, string>(),
				new RequestPayload("ContentType", "Data"),
				"traceId");

			var response = await mediator.Send(request);

			Assert.That(response.Body, Is.EqualTo("ResponseContent"));
		}
	}
}
