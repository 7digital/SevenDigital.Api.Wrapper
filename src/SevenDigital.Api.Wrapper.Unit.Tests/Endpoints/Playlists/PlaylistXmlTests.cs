using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema.Playlists;
using SevenDigital.Api.Schema.Playlists.Response.Endpoints;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Endpoints.Playlists
{
	[TestFixture]
	public class PlaylistXmlTests
	{
		private Playlist _playlist;
		
		[SetUp]
		public async void SetUp()
		{
			var requestBuilder = A.Fake<IRequestBuilder>();
			var httpClient = A.Fake<IHttpClient>();
			var responseParser = new ResponseParser(new ApiResponseDetector());

			var responseXml = File.ReadAllText("StubResponses/Playlist.xml");
			var validPlaylistsResponse = ResponseCreator.FromBody(HttpStatusCode.OK, responseXml);
			A.CallTo(() => httpClient.Send(null)).WithAnyArguments().Returns(Task.FromResult(validPlaylistsResponse));
			var fluentApi = new FluentApi<Playlist>(httpClient, requestBuilder, responseParser);

			_playlist = await fluentApi.Please();
		}

		[Test]
		public void then_the_playlist_has_been_deserialized_correctly()
		{
			Assert.That(_playlist.Id, Is.EqualTo("52b3733cc902160fa8bc9f34"));
			Assert.That(_playlist.Name, Is.EqualTo("Album From Catalogue"));
			Assert.That(_playlist.LastUpdated.ToString("dd/MM/yyyy hh:mm:ss"), Is.EqualTo(new DateTime(2014, 1, 22, 14, 52, 21).ToString("dd/MM/yyyy hh:mm:ss")));
			Assert.That(_playlist.Visibility, Is.EqualTo(PlaylistVisibilityType.Private));
		}

		[Test]
		public void _then_the_playlist_tracks_have_deserialized_correctly()
		{
			Assert.That(_playlist.Tracks.Count, Is.EqualTo(4));

			var firstTrack = _playlist.Tracks.FirstOrDefault();

			Assert.That(firstTrack.PlaylistItemId, Is.EqualTo("52cd88c2c902161660aeab80"));
			Assert.That(firstTrack.TrackId, Is.EqualTo("5495893"));
			Assert.That(firstTrack.TrackTitle, Is.EqualTo("No You Girls (Trentmoller Remix)"));
			Assert.That(firstTrack.TrackVersion, Is.EqualTo("Trentmoller Remix"));
			Assert.That(firstTrack.ArtistId, Is.Null);
			Assert.That(firstTrack.ArtistAppearsAs, Is.EqualTo("Franz Ferdinand"));
			Assert.That(firstTrack.ReleaseId, Is.EqualTo("496338"));
			Assert.That(firstTrack.ReleaseTitle, Is.EqualTo("No You Girls Remixes Part 2"));
			Assert.That(firstTrack.ReleaseArtistId, Is.Null);
			Assert.That(firstTrack.ReleaseArtistAppearsAs, Is.EqualTo("Franz Ferdinand"));
			Assert.That(firstTrack.ReleaseVersion, Is.EqualTo("Digital Download"));
			Assert.That(firstTrack.Source, Is.EqualTo("7digital"));
			Assert.That(firstTrack.AudioUrl, Is.EqualTo("http://stream.svc.7digital.net/stream/catalogue?trackId=5495893"));
			Assert.That(firstTrack.ImageUrl, Is.EqualTo("http://artwork-cdn.7static.com/static/img/sleeveart/00/027/190/0002719061_$size$.jpg"));
			Assert.That(firstTrack.User, Is.EqualTo("id:4874383"));
			Assert.That(firstTrack.DateAdded, Is.Null);
		}
	}
}
