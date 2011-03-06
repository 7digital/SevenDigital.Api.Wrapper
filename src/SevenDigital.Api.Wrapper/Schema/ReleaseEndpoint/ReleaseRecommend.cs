using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint
{
	[Serializable]
	[ApiEndpoint("release/recommend")]
	[XmlRoot("recommendations")]
	public class ReleaseRecommend
	{
		[XmlElement("page")]
		public int Page { get; set; }

		[XmlElement("pageSize")]
		public int PageSize { get; set; }

		[XmlElement("totalItems")]
		public int TotalItems { get; set; }

		[XmlElement("recommendedItem")]
		public List<RecommendedItem> RecommendedItems { get; set; }
	}
}