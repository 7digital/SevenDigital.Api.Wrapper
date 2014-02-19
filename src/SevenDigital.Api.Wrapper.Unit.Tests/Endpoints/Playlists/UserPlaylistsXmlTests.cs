using System;
using System.IO;
using System.Linq;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema.Playlists;
using SevenDigital.Api.Schema.Playlists.Response.Endpoints;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Endpoints.Playlists
{
	[TestFixture]
	public class UserPlaylistsXmlTests
	{
		private UserPlaylists _userPlaylists;

		[SetUp]
		public void SetUp()
		{
			var requestBuilder = A.Fake<IRequestBuilder>();
			var httpClient = A.Fake<IHttpClient>();
			var responseXml = File.ReadAllText("StubResponses/Playlists.xml");
			var validPlaylistsResponse = new Response(HttpStatusCode.OK, responseXml);
			A.CallTo(() => httpClient.Send(null)).WithAnyArguments().Returns(validPlaylistsResponse);
			var fluentApi = new FluentApi<UserPlaylists>(httpClient, requestBuilder);

			_userPlaylists = fluentApi.Please();
		}

		[Test]
		public void then_there_are_the_correct_amount_of_playlists()
		{
			Assert.That(_userPlaylists.TotalItems, Is.EqualTo(4));
			Assert.That(_userPlaylists.Playlists.Count, Is.EqualTo(4));
			Assert.That(_userPlaylists.Page, Is.EqualTo(1));
			Assert.That(_userPlaylists.PageSize, Is.EqualTo(10));
		}

		[Test]
		public void then_the_links_have_been_deserialized_correctly()
		{
			var firstOrDefault = _userPlaylists.Playlists.FirstOrDefault();
			Assert.That(firstOrDefault.Links.Count, Is.EqualTo(3));
		}

		[Test]
		public void then_everything_else_has_been_deserialized_correctly()
		{
			var firstOrDefault = _userPlaylists.Playlists.FirstOrDefault();
			Assert.That(firstOrDefault.Id, Is.EqualTo("52b3733cc902160fa8bc9f34"));
			Assert.That(firstOrDefault.LastUpdated.ToString("dd/MM/yyyy hh:mm:ss"), Is.EqualTo(new DateTime(2014, 1, 22, 14, 52, 21).ToString("dd/MM/yyyy hh:mm:ss"))); 
			Assert.That(firstOrDefault.Name, Is.EqualTo("Album From Catalogue"));
			Assert.That(firstOrDefault.TrackCount, Is.EqualTo(4));
			Assert.That(firstOrDefault.Visibility, Is.EqualTo(PlaylistVisibilityType.Private));
		}
	}
}