using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Schema.Artists
{
	[ApiEndpoint("artist/releases")]
	[XmlRoot("releases")]
	[DataContract(Name="releases")]
	[Serializable]
	public class ArtistReleases : HasPaging, HasArtistIdParameter, HasReleaseTypeParameter
	{
		[XmlElement("release")]
		[DataMember(Name="release")]
		public List<Release> Releases { get; set; }
	}
}