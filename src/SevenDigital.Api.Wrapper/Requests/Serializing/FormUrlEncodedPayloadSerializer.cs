using System;
using System.Collections;
using System.Linq;

namespace SevenDigital.Api.Wrapper.Requests.Serializing
{
	public class FormUrlEncodedPayloadSerializer : IPayloadSerializer
	{
		public PayloadFormat Handles
		{
			get { return PayloadFormat.FormUrlEncoded; }
		}

		public string ContentType
		{
			get { return "application/x-www-form-urlencoded"; }
		}

		public string Serialize<TPayload>(TPayload payload) where TPayload : class
		{
			return SerialiseWithRefection(payload);
		}

		/// <summary>
		/// Code to "Serialize object into a query string with Reflection" by Ole Michelsen 2012
		/// Will do most poco objects
		///  http://ole.michelsen.dk/blog/serialize-object-into-a-query-string-with-reflection.html
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		private string SerialiseWithRefection(object request)
		{
			// Get all properties on the object
			var properties = request.GetType().GetProperties()
				.Where(x => x.CanRead)
				.Where(x => x.GetValue(request, null) != null)
				.ToDictionary(x => x.Name, x => x.GetValue(request, null));

			// Get names for all IEnumerable properties (excl. string)
			var propertyNames = properties
				.Where(x => !(x.Value is string) && x.Value is IEnumerable)
				.Select(x => x.Key)
				.ToList();

			// Concat all IEnumerable properties into a comma separated string
			foreach (var key in propertyNames)
			{
				var valueType = properties[key].GetType();
				var valueElemType = valueType.IsGenericType
					? valueType.GetGenericArguments()[0]
					: valueType.GetElementType();
				if (valueElemType.IsPrimitive || valueElemType == typeof(string))
				{
					var enumerable = properties[key] as IEnumerable;
					properties[key] = string.Join(",", enumerable.Cast<object>());
				}
			}

			// Concat all key/value pairs into a string separated by ampersand
			return string.Join("&", properties
				.Select(x => string.Concat(
					Uri.EscapeDataString(x.Key), "=",
					Uri.EscapeDataString(x.Value.ToString()))));
		}
	}
}