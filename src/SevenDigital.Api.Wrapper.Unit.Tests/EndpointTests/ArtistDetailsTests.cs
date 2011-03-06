using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Repository;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Utility.Http;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointTests
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistDetailsTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver()); // TODO: Set up using castle?

			Artist artist = new FluentApi<Artist>(httpGetResolver)
				.WithParameter("artistid","1")
				.Resolve();
			
			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Name, Is.EqualTo("Keane"));
			Assert.That(artist.SortName, Is.EqualTo("Keane"));
			Assert.That(artist.Url, Is.EqualTo("http://www.7digital.com/artists/keane/?partner=1401"));
			Assert.That(artist.Image, Is.EqualTo("http://cdn.7static.com/static/img/artistimages/00/000/000/0000000001_150.jpg"));
		}
	}

	[TestFixture]
	[Category("Integration")]
	public class ArtistBrowseTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver()); // TODO: Set up using castle?

			ArtistBrowse artistBrowse = new FluentApi<ArtistBrowse>(httpGetResolver).WithParameter("letter", "radio").Resolve();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(1));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(10));
			Assert.That(artistBrowse.Artists.Count,Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver()); // TODO: Set up using castle?

			ArtistBrowse artistBrowse = new FluentApi<ArtistBrowse>(httpGetResolver)
											.WithParameter("letter", "radio")
											.WithParameter("page", "2")
											.WithParameter("pageSize", "20")
											.Resolve();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}