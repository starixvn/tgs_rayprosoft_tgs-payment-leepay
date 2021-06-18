using System.Text.Json.Serialization;

namespace Tgs.Payment.LeePay
{
	internal class QueryExchangeRateOrder
	{
		public string From { get; set; } = default!;

		public string To { get; set; } = default!;

		[JsonPropertyName("txnSign")]
		public string TranscationSigningCode { get; set; } = default!;
	}
}
