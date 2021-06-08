using System.Text.Json.Serialization;

namespace Tgs.Payment.LeePay
{
	internal class ExchangeRateOrder
	{
		[JsonPropertyName("from")]
		public string From { get; set; } = default!;

		[JsonPropertyName("to")]
		public string To { get; set; } = default!;

		[JsonPropertyName("txnSign")]
		public string TranscationSigningCode { get; set; } = default!;
	}
}