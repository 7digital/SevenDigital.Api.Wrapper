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
	public class PlaylistParameterExtensionTests
	{
		private FluentApi<Playlist> _fluentApi;
		private IRequestBuilder _requestBuilder;

		[SetUp]
		public void SetUp()
		{
			_requestBuilder = A.Fake<IRequestBuilder>();
			var httpClient = A.Fake<IHttpClient>();
			var responseParser = A.Fake<IResponseParser>();

			_fluentApi = new FluentApi<Playlist>(httpClient, _requestBuilder, responseParser);
		}

		[Test]
		public void Correct_playlistId_is_passed_to_fluentapi()
		{
			const string expectedPlaylistId = "test";

			_fluentApi.ForPlaylistId(expectedPlaylistId).Response();

			Expression<Func<Request>> specification = () => _requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Parameters["playlistId"] == expectedPlaylistId));

			A.CallTo(specification).MustHaveHappened();
		}

		[Test]
		public void Correct_playlistId_is_pulled_from_uri()
		{
			const string expectedPlaylistId = "test";

			var playlistUrl = new Uri("http://api.7digital.com/1.2/playlists/"+expectedPlaylistId);
			_fluentApi.ForPlaylistUrl(playlistUrl).Response();

			Expression<Func<Request>> specification = () => _requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Parameters["playlistId"] == expectedPlaylistId));

			A.CallTo(specification).MustHaveHappened();
		}
	}
}