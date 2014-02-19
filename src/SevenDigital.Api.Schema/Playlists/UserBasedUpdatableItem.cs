
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Playlists
{
	[DataContract]
	public class UserBasedUpdatableItem
	{
		public string UserId { get; set; }

		[XmlElement("lastUpdated")]
		public DateTime LastUpdated { get; set; }
	}
}