using System;
using System.Xml;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public class ApiXmlException : Exception
	{
		public Error Error { get; set; }
		public ApiXmlException(string message, XmlNode errorXml) : base(message)
		{
			var xmlSerializer = new XmlSerializer<Error>();
			Error = xmlSerializer.DeSerialize(errorXml);
		}
	}
}