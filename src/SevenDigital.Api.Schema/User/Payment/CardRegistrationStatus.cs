
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.User.Payment
{
	public enum CardRegistrationStatus
	{
		[XmlEnum(Name = "pending")]
		Pending,
		[XmlEnum(Name = "complete")]
		Complete,
		[XmlEnum(Name = "error")]
		Error,
	}
}
