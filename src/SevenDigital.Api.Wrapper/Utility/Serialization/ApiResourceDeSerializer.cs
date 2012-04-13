using System;
using System.IO;
using System.Xml.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public class ApiResourceDeSerializer<T> : IDeSerializer<T> where T : class
	{
		public T DeSerialize(string response)
		{
			var ser = new XmlSerializer(typeof(T));
			using (var reader = new StringReader(response))
			{
				try
				{
					object obj = ser.Deserialize(reader);
					var instance = (T)obj;
					return instance;
				}
				catch(InvalidOperationException ioex)
				{
					if (typeof(T) == typeof(Error))
						throw new ApiXmlException("Error trying to deserialize xml response", response);

					throw new ApiXmlException("Error trying to deserialize error xml response", ioex);
				}
			}
		}
	}
}
