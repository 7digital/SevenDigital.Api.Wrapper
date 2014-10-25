using NUnit.Framework;

using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests
{
	[TestFixture]
	public class UriPathTests
	{
		[Test]
		public void Should_combine_paths_when_neither_has_slash()
		{
			var fullPath = UriPath.Combine("foo", "bar");

			Assert.That(fullPath, Is.EqualTo("foo/bar"));
		}

		[Test]
		public void Should_combine_paths_when_first_has_slash()
		{
			var fullPath = UriPath.Combine("foo/", "bar");

			Assert.That(fullPath, Is.EqualTo("foo/bar"));
		}

		[Test]
		public void Should_combine_paths_when_second_has_slash()
		{
			var fullPath = UriPath.Combine("foo", "/bar");

			Assert.That(fullPath, Is.EqualTo("foo/bar"));
		}

		[Test]
		public void Should_combine_paths_when_both_have_slashes()
		{
			var fullPath = UriPath.Combine("foo/", "/bar");

			Assert.That(fullPath, Is.EqualTo("foo/bar"));
		}

		[Test]
		public void Should_cope_with_empty_base()
		{
			var fullPath = UriPath.Combine(string.Empty, "foo/bar");

			Assert.That(fullPath, Is.EqualTo("foo/bar"));
		}

		[Test]
		public void Should_cope_with_empty_rest()
		{
			var fullPath = UriPath.Combine("foo/bar", string.Empty);

			Assert.That(fullPath, Is.EqualTo("foo/bar"));
		}
	}
}
