﻿using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions
{
	[Serializable]
	internal class NonXmlContentException : SerializationException
	{
		const string DEFAULT_ERROR_MESSAGE = "Unexpected non-XML content found when deserializing";

		public NonXmlContentException(Exception innerException)
			: base(DEFAULT_ERROR_MESSAGE, innerException)
		{
		}

		protected NonXmlContentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
