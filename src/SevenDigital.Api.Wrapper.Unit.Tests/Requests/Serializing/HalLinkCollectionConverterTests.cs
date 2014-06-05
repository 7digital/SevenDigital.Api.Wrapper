using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Newtonsoft.Json;
using SevenDigital.Api.Schema.Playlists.Response;
using SevenDigital.Api.Wrapper.Requests.Serializing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests.Serializing
{
	[TestFixture]
	public class HalLinkCollectionConverterTests
	{
		private HalLinkCollectionConverter _halLinkCollectionConverter;

		[SetUp]
		public void Setup()
		{
			_halLinkCollectionConverter = new HalLinkCollectionConverter();
		}

		[Test]
		public void can_convert_list_of_links()
		{
			var canConvert = _halLinkCollectionConverter.CanConvert(typeof(List<Link>));
			Assert.That(canConvert);
		}

		[Test]
		public void cant_convert_other_object()
		{
			var canConvert = _halLinkCollectionConverter.CanConvert(typeof(object));
			Assert.That(canConvert, Is.False);
		}

		[Test]
		public void can_serialize_one_link()
		{
			var stringBuilder = new StringBuilder();
			var jsonTextWriter = new JsonTextWriter(new StringWriter(stringBuilder));
			var jsonSerializer = new JsonSerializer();

			var value = new List<Link>
				{
					new Link("self", "http://a/url")
				};

			_halLinkCollectionConverter.WriteJson(jsonTextWriter, value, jsonSerializer);

			const string expected = "{\"self\":{\"href\":\"http://a/url\"}}";
			Assert.That(stringBuilder.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void can_serialize_multiple_links()
		{
			var stringBuilder = new StringBuilder();
			using (var jsonTextWriter = new JsonTextWriter(new StringWriter(stringBuilder)))
			{
				var jsonSerializer = new JsonSerializer();

				var value = new List<Link>
				{
					new Link("self", "http://a/url"), 
					new Link("details", "http://a/nother/url")
				};

				_halLinkCollectionConverter.WriteJson(jsonTextWriter, value, jsonSerializer);

				const string expected = "{\"self\":{\"href\":\"http://a/url\"},\"details\":{\"href\":\"http://a/nother/url\"}}";
				Assert.That(stringBuilder.ToString(), Is.EqualTo(expected));
			}
		}

		[Test]
		public void deserailzaes_multiple()
		{
			const string json = "{\"self\":{\"href\":\"http://a/url\"},\"details\":{\"href\":\"http://a/nother/url\"}}";
			var stringReader = new StringReader(json);

			using (var jsonTextReader = new JsonTextReader(stringReader))
			{
				var readJson = _halLinkCollectionConverter.ReadJson(jsonTextReader, typeof(List<Link>), null, new JsonSerializer());

				Assert.That(readJson, Is.TypeOf<List<Link>>());

				var links = readJson as List<Link>;
				Assert.That(links.Count, Is.EqualTo(2));
				Assert.That(links[0].Rel, Is.EqualTo("self"));
				Assert.That(links[0].Href, Is.EqualTo("http://a/url"));

				Assert.That(links[1].Rel, Is.EqualTo("details"));
				Assert.That(links[1].Href, Is.EqualTo("http://a/nother/url"));
			}
		}
	}
}