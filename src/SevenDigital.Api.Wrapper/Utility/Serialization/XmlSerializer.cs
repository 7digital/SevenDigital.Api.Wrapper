using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public class XmlSerializer<T> : ISerializer<T> where T : class
	{
		public T DeSerialize(XmlNode document) {
			
			var reader = new XmlNodeReader(document);
			var ser = new XmlSerializer(typeof(T));
			object obj = ser.Deserialize(reader);
			var instance = (T)obj;
			return instance;
		}

		public IXPathNavigable Serialize(T serializableObject) {
			if (serializableObject == null)
				throw new ArgumentNullException("serializableObject");

			Type obj = serializableObject.GetType();

			if (!obj.IsSerializable)
				throw new ArgumentException(String.Format("The object passed is not serializable: {0}", obj.Name));

			var doc = new XmlDocument();

			using (var s = new MemoryStream()) {
				var x = new XmlSerializer(serializableObject.GetType());
				x.Serialize(s, serializableObject);
				s.Position = 0;
				doc.Load(s);
				s.Close();
			}

			return doc;
		}
	}
}