using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace Tgs.Payment.LeePay.Tests
{
	[TestFixture]
	public class HasherTests
	{
		[Test]
		public void DateTimeOffset_Hash_Should_Match_Format()
		{
			var sut = new LeePayHasher();

			var systemId = "systemId";
			var systemKey = "systemKey";

			var dict = new Dictionary<string, object>
			{
				["date"] = DateTimeOffset.Parse("2021-05-07T06:39:14.530Z")
			};

			var expected = HashCode(systemId, systemKey, "date=2021-05-07T06:39:14.530Z");

			var actual = sut.ComputeHash(systemId, systemKey, dict);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Null_Value_Should_Transfer_To_null_String()
		{
			var sut = new LeePayHasher();

			var systemId = "systemId";
			var systemKey = "systemKey";

			var dict = new Dictionary<string, object>
			{
				["data"] = null
			};

			var expected = HashCode(systemId, systemKey, "data=null");

			var actual = sut.ComputeHash(systemId, systemKey, dict);

			Assert.AreEqual(expected, actual);
		}

		private string HashCode(string systemId, string systemKey, string content)
		{
			var fullText = $"SYSTEM_ID={systemId}&{content}&SYSTEM_KEY={systemKey}";

			var base64Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(fullText));

			var hash = string.Join(
				string.Empty,
				SHA256.HashData(Encoding.UTF8.GetBytes(base64Code)).Select(b => b.ToString("X2")));

			return hash;
		}
	}
}
