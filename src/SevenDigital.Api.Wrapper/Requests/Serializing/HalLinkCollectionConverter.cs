using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SevenDigital.Api.Schema.Playlists.Response;

namespace SevenDigital.Api.Wrapper.Requests.Serializing
{
	/// <summary>
	/// http://stateless.co/hal_specification.html
	/// NOTE: Only takes card of the _links portion, does not support curies and templated links yet
	/// </summary>
	public class HalLinkCollectionConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var collection = value as List<Link>;
			if (collection != null)
			{
				writer.WriteStartObject();
				foreach (var link in collection)
				{
					writer.WritePropertyName(link.Rel);
					writer.WriteStartObject();
					writer.WritePropertyName("href");
					writer.WriteValue(link.Href);
					writer.WriteEndObject();
				}
				writer.WriteEndObject();
			}
			else
			{
				writer.WriteNull();
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var jsonTokens = JToken.ReadFrom(reader);
			var allLinksAsJProperties = jsonTokens.Select(token => (JProperty)token);
			var allLinks = allLinksAsJProperties.Select(link => new Link(link.Name, link.Value["href"].ToString()));
			return allLinks.ToList();
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(List<Link>).IsAssignableFrom(objectType);
		}
	}
}