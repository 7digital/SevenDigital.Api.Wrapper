namespace SevenDigital.Api.Wrapper.Requests
{
	public class AcceptFormat
	{
		private static readonly AcceptFormat xmlOnly = new AcceptFormat("application/xml");
		private static readonly AcceptFormat jsonPrefered = new AcceptFormat("application/json, application/xml;q=0.5"); 

		private readonly string _acceptHeader;

		public AcceptFormat(string acceptHeader)
		{
			_acceptHeader = acceptHeader;
		}

		public static AcceptFormat XmlOnly { get { return xmlOnly; } }
		public static AcceptFormat JsonPrefered { get { return jsonPrefered; } }

		public override string ToString()
		{
			return _acceptHeader;
		}
	}
}
