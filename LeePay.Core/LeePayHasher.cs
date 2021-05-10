using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tgs.Payment.LeePay
{
	public class LeePayHasher
	{
		public string ComputeHash(string systemId, string systemKey, IDictionary<string, object> dict)
		{
			var content = string.Join(
				"&",
				dict.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value ?? "null"}"));

			var fullText = $"SYSTEM_ID={systemId}&{content}&SYSTEM_KEY={systemKey}";

			var base64Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(fullText));

			var hash = string.Join(
				string.Empty,
				SHA256.HashData(Encoding.UTF8.GetBytes(base64Code)).Select(b => b.ToString("X2")));

			return hash;
		}
	}
}
