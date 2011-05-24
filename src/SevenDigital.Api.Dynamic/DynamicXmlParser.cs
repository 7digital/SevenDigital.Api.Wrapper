using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace SevenDigital.Api.Dynamic
{
	public class DynamicXmlParser : DynamicObject, IEnumerable
	{
		private readonly List<XElement> _elements;

		public DynamicXmlParser(string text)
		{
			var doc = XDocument.Parse(text);
			_elements = new List<XElement> { doc.Root };
		}

		protected DynamicXmlParser(XElement element)
		{
			_elements = new List<XElement> { element };
		}

		protected DynamicXmlParser(IEnumerable<XElement> elements)
		{
			_elements = new List<XElement>(elements);
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = null;
			switch (binder.Name)
			{
				case "Value":
					result = _elements[0].Value;
					break;
				case "Count":
					result = _elements.Count;
					break;
				default:
					var attr = _elements[0].Attribute(XName.Get(binder.Name));
					if (attr == null)
					{
						var items = _elements.Descendants(XName.Get(binder.Name));
						if (items == null || items.Count() == 0)
							return false;
						result = new DynamicXmlParser(items);
						break;
					}
					result = attr;
					break;
			}
			return true;
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			var ndx = (int)indexes[0];
			result = new DynamicXmlParser(_elements[ndx]);
			return true;
		}

		public IEnumerator GetEnumerator()
		{
			return _elements.Select(element => new DynamicXmlParser(element)).GetEnumerator();
		}
	}
}
