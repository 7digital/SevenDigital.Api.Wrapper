using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Exceptions
{
	public class RoundTripSerialiser
	{
		public T RoundTrip<T>(T exception)
		{
			var binaryFormatter = new BinaryFormatter();
			using (var stream = new MemoryStream())
			{
				binaryFormatter.Serialize(stream, exception);
				stream.Seek(0, 0);
				exception = (T)binaryFormatter.Deserialize(stream);
			}
			return exception;
		}
	}
}
