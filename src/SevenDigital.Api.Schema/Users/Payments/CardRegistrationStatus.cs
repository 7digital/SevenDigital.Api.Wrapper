
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Users.Payment
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
