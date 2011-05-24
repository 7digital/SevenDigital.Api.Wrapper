using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Tags
{
	[Serializable]
	[ApiEndpoint("artist/bytag/top")]
	[XmlRoot("taggedResults")]
	public class ArtistByTagTop : HasPaging
	{
		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("taggedItem")]
		public List<TaggedArtists> TaggedArtists { get; set; }
	}

	[Serializable]
	[ApiEndpoint("release/bytag/new")]
	[XmlRoot("taggedResults")]
	public class ReleaseByTagNew : HasPaging
	{
		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("taggedItem")]
		public List<TaggedReleases> TaggedReleases { get; set; }
	}

	[Serializable]
	[ApiEndpoint("release/bytag/top")]
	[XmlRoot("taggedResults")]
	public class ReleaseByTagTop : HasPaging
	{
		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("taggedItem")]
		public List<TaggedReleases> TaggedReleases { get; set; }
	}
}