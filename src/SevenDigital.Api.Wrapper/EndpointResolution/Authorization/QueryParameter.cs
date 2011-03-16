namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	internal class QueryParameter
	{
		public string Name { get; private set; }
		public string Value { get; private set; }

		public QueryParameter(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}