using System.IO;
using System.Text;

namespace SevenDigital.Api.Wrapper.Requests.Serializing
{
	public class Utf8StringWriter : StringWriter
	{
		public override Encoding Encoding
		{
			get { return Encoding.UTF8; }
		}
	}
}