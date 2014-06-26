﻿using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Artists
{
	[ApiEndpoint("artist/details")]
	[XmlRoot("artist")]
	[Serializable]
	public class Artist : HasArtistIdParameter
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("sortName")]
		public string SortName { get; set; }

		[XmlElement("appearsAs")]
		public string AppearsAs { get; set; }

		[XmlElement("image")]
		public string Image { get; set; }

		[XmlElement("url")]
		public string Url { get; set; }
	}
}