using System;
using System.Linq.Expressions;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema.Playlists.Response.Endpoints;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Endpoints.Playlists
{
	[TestFixture]
	public class PlaylistItemIdParameterExtensionTests
	{
		private FluentApi<PlaylistItem> _fluentApi;
		private IRequestBuilder _requestBuilder;

		[SetUp]
		public void SetUp()
		{
			_requestBuilder = A.Fake<IRequestBuilder>();
			var httpClient = A.Fake<IHttpClient>();
			var responseParser = A.Fake<IResponseParser>();

			_fluentApi = new FluentApi<PlaylistItem>(httpClient, _requestBuilder, responseParser);
		}

		[Test]
		public void Correct_playlistItemId_is_passed_to_fluentapi()
		{
			const string expectedPlaylistItemId = "test";

			_fluentApi.ForPlaylistItemId(expectedPlaylistItemId).Response();

			Expression<Func<Request>> specification = () => _requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Parameters["playlistItemId"] == expectedPlaylistItemId));

			A.CallTo(specification).MustHaveHappened();
		}
	}
}