using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.ReleaseEndpoint
{
	
	[ApiEndpoint("release/recommend")]
	[XmlRoot("recommendations")]
	public class ReleaseRecommend : HasPaging, HasReleaseIdParameter
	{
		[XmlElement("recommendedItem")]
		public List<RecommendedItem> RecommendedItems { get; set; }
	}
}