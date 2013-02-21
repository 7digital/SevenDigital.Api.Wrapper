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
		public TextItem Review { get; set; }

		[XmlElement("promotionalText")]
		public TextItem PromotionalText { get; set; }
	}

	public class TextItem
	{
		[XmlElement("text")]
		public string Text { get; set; }
	}
}
