using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SevenDigital.Api.Wrapper.Requests.Serializing
{
	public class JsonPayloadSerializer : IPayloadSerializer
	{
		private static JsonSerializer _serializer;

		public JsonPayloadSerializer()
		{
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new List<JsonConverter> { new StringEnumConverter(), new HalLinkCollectionConverter(), new IsoDateTimeConverter() }
			};

			_serializer = JsonSerializer.Create(settings);
		}

		public string ContentType { get { return "application/json"; } }

		public string Serialize<TPayload>(TPayload payload) where TPayload : class
		{
			var stringWriter = new StringWriter();
			using (var writer = new JsonTextWriter(stringWriter))
			{
				_serializer.Serialize(writer, payload);
			}
			return stringWriter.ToString();
		}
	}
}