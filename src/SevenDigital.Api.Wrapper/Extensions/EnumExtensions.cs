using System;
using System.Reflection;
using System.ComponentModel;

namespace SevenDigital.Api.Wrapper.Extensions
{
	public static class EnumExtensions
	{
		public static string GetDescription(this Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Description;

			return value.ToString();
		}
	}
}
