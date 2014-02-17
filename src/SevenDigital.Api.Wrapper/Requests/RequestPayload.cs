namespace SevenDigital.Api.Wrapper.Requests
{
	public class RequestPayload
	{
		private readonly string _contentType;
		private readonly string _data;

		public RequestPayload(string contentType, string data)
		{
			_contentType = contentType;
			_data = data;
		}

		public string ContentType
		{
			get { return _contentType; }
		}

		public string Data
		{
			get { return _data; }
		}
	}
}