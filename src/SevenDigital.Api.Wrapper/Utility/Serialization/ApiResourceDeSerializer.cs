using System.IO;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
    public class ApiResourceDeSerializer<T> : IDeSerializer<T> where T : class
    {
        public T DeSerialize(string response)
        {
            var ser = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(response))
            {
                object obj = ser.Deserialize(reader);
                var instance = (T)obj;
                return instance;
            }
        }
    }
}
