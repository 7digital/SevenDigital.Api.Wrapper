using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.ReleaseEndpoint
{
	[XmlRoot("editorial")]
	[ApiEndpoint("release/editorial")]
	[Serializable]
	public class ReleaseEditorial : HasReleaseIdParameter
	{
		[XmlElement("review")]
		public Review Review { get; set; }

		[XmlElement("staffRecommendation")]
		public Review StaffRecommendation { get; set; }

		[XmlElement("promotionalText")]
		public string PromotionalText { get; set; }
	}

	public class Review
	{
		[XmlElement("text")]
		public string Text { get; set; }
	}
}
