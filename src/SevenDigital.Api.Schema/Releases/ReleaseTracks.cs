using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.Tracks;

namespace SevenDigital.Api.Schema.Releases
{
	[XmlRoot("tracks")]
	[ApiEndpoint("release/tracks")]
	[Serializable]
	public class ReleaseTracks : HasPaging, HasReleaseIdParameter
	{
		[XmlElement("track")]
		public List<Track> Tracks{ get; set; }
	}
}
