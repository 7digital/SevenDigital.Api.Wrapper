using System.Collections.Generic;
using NUnit.Framework;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Wrapper.Requests.Serializing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests.Serializing
{
	public class DataWithIntValues
	{
		public string Name { get; set; }
		public List<int> Values { get; set; }
	}

	[TestFixture]
	public class FormUrlEncodedPayloadSerializerTests
	{
		private FormUrlEncodedPayloadSerializer _payloadSerializer;

		[SetUp]
		public void SetUp()
		{
			_payloadSerializer = new FormUrlEncodedPayloadSerializer();
		}

		[Test]
		public void Should_have_correct_contenttype()
		{
			Assert.That(_payloadSerializer.ContentType, Is.EqualTo("application/x-www-form-urlencoded"));
		}

		[Test]
		public void Should_serialize_artist_as_expected()
		{
			const string expectedEncodedOutput = "Id=143451&Name=MGMT&AppearsAs=MGMT&Image=http%3A%2F%2Fcdn.7static.com%2Fstatic%2Fimg%2Fartistimages%2F00%2F001%2F434%2F0000143451_150.jpg&Url=http%3A%2F%2Fwww.7digital.com%2Fartist%2Fmgmt%2F%3Fpartner%3D1401";

			var artist = new Artist
			{
				AppearsAs = "MGMT",
				Name = "MGMT",
				Id = 143451,
				Image = "http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg",
				Url = "http://www.7digital.com/artist/mgmt/?partner=1401"
			};

			var actual = _payloadSerializer.Serialize(artist);

			Assert.That(actual, Is.EqualTo(expectedEncodedOutput));
		}


		[Test]
		public void Should_serialize_data_with_ints_as_expected()
		{
			const string expectedEncodedOutput = "Name=test%20data&Values=1%2C2%2C12345";

			var data = new DataWithIntValues
			{
				Name = "test data",
				Values = new List<int> {1, 2, 12345}
			};

			var actual = _payloadSerializer.Serialize(data);

			Assert.That(actual, Is.EqualTo(expectedEncodedOutput));
		}
	}
}
