using System.Xml.Serialization;

namespace SevenDigital.Api.Schema
{
	public abstract class HasPaging
	{
		[XmlElement("page")]
		public int Page { get; set; }

		[XmlElement("pageSize")]
		public int PageSize { get; set; }

		[XmlElement("totalItems")]
		public int TotalItems { get; set; }
	}
}