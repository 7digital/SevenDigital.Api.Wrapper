using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.Authorization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.Authorization
{
	[TestFixture]
	public class TimeStampGeneratorTests
	{
		[Test]
		public void Should_generate_timestamp()
		{
			var timeStampGenerator = new TimeStampGenerator();
			string generate = timeStampGenerator.Generate();
			Assert.That(generate, Is.Not.Empty);

			int timestamp = Convert.ToInt32(generate);
			Assert.That(timestamp, Is.GreaterThan(0));
		}
	}

	[TestFixture]
	public class QueryParameterComparerTests
	{
		[Test]
		public void Go_compare()
		{
			var queryParameterA = new QueryParameter("two", "1");
			var queryParameterB = new QueryParameter("zed", "z");
			var queryParameterComparer = new QueryParameterComparer();
			Assert.That(queryParameterComparer.Compare(queryParameterA, queryParameterB), Is.EqualTo(-1));
			Assert.That(queryParameterComparer.Compare(queryParameterB, queryParameterA), Is.EqualTo(1));
			Assert.That(queryParameterComparer.Compare(queryParameterA, queryParameterA), Is.EqualTo(0));
		}

		[Test]
		public void Should_put_in_correct_order()
		{
			var queryParameters = new List<QueryParameter>
			                      	{
			                      		new QueryParameter("two", "1"),
										new QueryParameter("zed", "z"),
			                      		new QueryParameter("one", "2"),
			                      	};
			queryParameters.Sort(new QueryParameterComparer());
			Assert.That(queryParameters.ElementAt(0).Name, Is.EqualTo("one"));
			Assert.That(queryParameters.ElementAt(1).Name, Is.EqualTo("two"));
			Assert.That(queryParameters.ElementAt(2).Name, Is.EqualTo("zed"));
		}
	}
}