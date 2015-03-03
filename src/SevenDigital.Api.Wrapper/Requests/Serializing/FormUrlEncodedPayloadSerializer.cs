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
			return SerialiseWithReflection(payload);
		}

		/// <summary>
		/// Code to "Serialize object into a query string with Reflection" by Ole Michelsen 2012
		/// Will do most poco objects
		///  http://ole.michelsen.dk/blog/serialize-object-into-a-query-string-with-reflection.html
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		private string SerialiseWithReflection(object request)
		{
			var allProperties = request.GetType().GetProperties()
				.Where(x => x.CanRead)
				.Where(x => x.GetValue(request, null) != null)
				.ToDictionary(x => x.Name, x => x.GetValue(request, null));

			var propertyNames = allProperties
				.Where(x => !(x.Value is string) && x.Value is IEnumerable)
				.Select(x => x.Key)
				.ToList();

			foreach (var key in propertyNames)
			{
				var valueType = allProperties[key].GetType();
				var valueElemType = valueType.IsGenericType
					? valueType.GetGenericArguments()[0]
					: valueType.GetElementType();

				if (valueElemType.IsPrimitive || valueElemType == typeof(string))
				{
					var enumerable = allProperties[key] as IEnumerable;
					allProperties[key] = string.Join(",", enumerable.Cast<object>());
				}
			}

			var pairs = allProperties.Select(x =>
				string.Concat(
					Uri.EscapeDataString(x.Key), "=", 
					Uri.EscapeDataString(x.Value.ToString())));

			return string.Join("&", pairs);
		}
	}
}