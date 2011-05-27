using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.ReleaseEndpoint
{
	[Serializable]
	[ApiEndpoint("release/recommend")]
	[XmlRoot("recommendations")]
	public class ReleaseRecommend : HasPaging
	{
		[XmlElement("recommendedItem")]
		public List<RecommendedItem> RecommendedItems { get; set; }
	}
}