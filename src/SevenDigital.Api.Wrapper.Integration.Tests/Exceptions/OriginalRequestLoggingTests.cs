using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.Exceptions
{
	[TestFixture]
	public class OriginalRequestLoggingTests
	{
		private InputParameterException _apiXmlException;

		[TestFixtureSetUp]
		public void SetUp()
		{
			// -- Deliberate error response
			Console.WriteLine("Trying artist/details without artistId parameter...");
			_apiXmlException = Assert.Throws<InputParameterException>(() => Api<Artist>.Create.WithParameter("test", "true").Please());
		}

		[Test]
		public void Can_access_the_original_request_uri_and_querystring_in_exception()
		{
			Assert.That(_apiXmlException.OriginalRequest.Url, Is.EqualTo("http://api.7digital.com/1.2/artist/details?test=true"));

			Console.WriteLine("Url: {0}", _apiXmlException.OriginalRequest.Url);
		}

		[Test]
		public void Can_access_the_original_request_headers_in_exception()
		{
			var originalRequestHeaders = _apiXmlException.OriginalRequest.Headers;

			Assert.That(originalRequestHeaders, Is.Not.Null);
			Assert.That(originalRequestHeaders["Authorization"], Is.Not.Null);
			Assert.That(originalRequestHeaders["Accept"], Is.Not.Null);

			Console.WriteLine("Authorization: {0}", originalRequestHeaders["Authorization"]);
			Console.WriteLine("Accept: {0}", originalRequestHeaders["Accept"]);
		}

		[Test]
		public void Can_access_the_original_request_method_in_exception()
		{
			Assert.That(_apiXmlException.OriginalRequest.Method, Is.EqualTo(HttpMethod.Get));
		}
	}
}