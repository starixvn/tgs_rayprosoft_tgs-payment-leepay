using System;
using System.Collections.Generic;
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

			var expected = "C733A72537156F035A8C4CF04EE1AF58808A71FB1DC47F3081F6813F4F503044";

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

			var expected = "C10F2D65A24B2DCBA95C789668FBF0B200E5166EB9226C07F84BF43ECA1B9C57";

			var actual = sut.ComputeHash(systemId, systemKey, dict);

			Assert.AreEqual(expected, actual);
		}
	}
}
