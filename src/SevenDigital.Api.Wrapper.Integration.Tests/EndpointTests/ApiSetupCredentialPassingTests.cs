using System;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests
{
	public class ApiWithCredentials : IApi
	{
		private readonly AppSettingsCredentials _credentials;
		private readonly ApiUri _apiUrl;

		public ApiWithCredentials()
		{
			_credentials = new AppSettingsCredentials();
			_apiUrl = new ApiUri();
		}

		public IFluentApi<T> Create<T>() where T : class, new()
		{
			return new FluentApi<T>(new HttpClientMediator(), new RequestBuilder(_apiUrl, _credentials), new ResponseParser(new ApiResponseDetector()));
		}
	}

	[TestFixture]
	public class ApiSetupCredentialPassingTests
	{
		[Test]
		public async void Can_hit_endpoint_if_I_pass_credentials_into_api()
		{
			IApi api = new ApiWithCredentials();
			var request = api.Create<Status>();
			var status = await request.Please();

			Assert.That(status, Is.Not.Null);
			Assert.That(status.ServerTime.Day, Is.EqualTo(DateTime.Now.Day));
		}

		[Test]
		public async void Can_hit_endpoint_if_I_pass_credentials_into_static_api()
		{
			StaticApiFactory.Factory = new ApiWithCredentials();

			var request = Api<Status>.Create;
			var status = await request.Please();

			Assert.That(status, Is.Not.Null);
			Assert.That(status.ServerTime.Day, Is.EqualTo(DateTime.Now.Day));
		}
	}
}