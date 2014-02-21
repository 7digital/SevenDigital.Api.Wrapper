using System.Collections.Generic;
using System.IO;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema.Playlists;
using SevenDigital.Api.Schema.Playlists.Requests;
using SevenDigital.Api.Schema.Playlists.Response.Endpoints;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Requests.Serializing;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Endpoints.Playlists
{
	[TestFixture]
	public class PlaylistPayloadDeserializationTests 
	{
		private FluentApi<UserPlaylists> _fluentApi;
		private IRequestBuilder _requestBuilder;

		[SetUp]
		public void SetUp()
		{
			_requestBuilder = A.Fake<IRequestBuilder>();
			var httpClient = A.Fake<IHttpClient>();
			var responseXml = File.ReadAllText("StubResponses/Playlists.xml");
			var validPlaylistsResponse = new Response(HttpStatusCode.OK, responseXml);
			A.CallTo(() => httpClient.Send(null)).WithAnyArguments().Returns(validPlaylistsResponse);
			_fluentApi = new FluentApi<UserPlaylists>(httpClient, _requestBuilder);

		}

		[Test]
		public void Should_serialize_playlist_as_expected()
		{
			var expectedXmlOutput = File.ReadAllText("StubRequests/Playlist.xml").Replace("\r\n", "").Replace("\t", "");
			var playlist = new PlaylistRequest
			{
				Name = "Test Playlist",
				Visibility = PlaylistVisibilityType.Private,
				Tracks = new List<Product>
				{
					new Product
					{
						ArtistAppearsAs = "MGMT",
						ArtistId = "123",
						AudioUrl = "test.mp3",
						ReleaseArtistAppearsAs = "MGMT",
						ReleaseArtistId = "123",
						ReleaseId = "123",
						ReleaseTitle = "Oracula Spectacular",
						ReleaseVersion = "extended",
						Source = "local",
						TrackId = "123",
						TrackTitle = "Weekend Wars",
						TrackVersion = "deluxe",
						ImageUrl = "http://my.image.com/image.jpg"
					}
				}
			};

			_fluentApi.WithPayload(playlist);

			var xml = playlist.ToXml();

			Assert.That(xml, Is.EqualTo(expectedXmlOutput));
		}

		[Test]
		public void Should_serialize_track_list_as_expected()
		{
			var expectedXmlOutput = File.ReadAllText("StubRequests/PlaylistsTracks.xml").Replace("\r\n", "").Replace("\t","");

			var products = new List<Product>
			{
				new Product
				{
					ArtistAppearsAs = "MGMT",
					ArtistId = "123",
					AudioUrl = "test.mp3",
					ReleaseArtistAppearsAs = "MGMT",
					ReleaseArtistId = "123",
					ReleaseId = "123",
					ReleaseTitle = "Oracula Spectacular",
					ReleaseVersion = "extended",
					Source = "local",
					TrackId = "123",
					TrackTitle = "Weekend Wars",
					TrackVersion = "deluxe",
					ImageUrl = "http://my.image.com/imageA.jpg"
				},
				new Product
				{
					ArtistAppearsAs = "MGMT",
					ArtistId = "123",
					AudioUrl = "test.mp3",
					ReleaseArtistAppearsAs = "MGMT",
					ReleaseArtistId = "123",
					ReleaseId = "123",
					ReleaseTitle = "Oracula Spectacular",
					ReleaseVersion = "extended",
					Source = "local",
					TrackId = "124",
					TrackTitle = "Kids",
					TrackVersion = "deluxe",
					ImageUrl = "http://my.image.com/imageB.jpg"
				}
			};

			var playlistTracksRequest = new PlaylistTracksRequest { Tracks = products };

			var xml = playlistTracksRequest.ToXml();

			Assert.That(xml, Is.EqualTo(expectedXmlOutput));
		}
	}
}