using System;
using System.Xml;
using SevenDigital.Api.Wrapper.DTO;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Repository
{
	public class ApiException : Exception
	{
		public Error Error { get; set; }
		public ApiException(string message, XmlNode errorXml) : base(message)
		{
			var xmlSerializer = new XmlSerializer<Error>();
			Error = xmlSerializer.DeSerialize(errorXml);
		}
	}
}