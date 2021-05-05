using System;
using System.Text.Json.Serialization;

namespace Tgs.Payment.LeePay.ViewModels
{
	public record WithdrawResponseViewModel
	{
		public DateTimeOffset CreatedAt { get; init; }

		public string CustomOrderNo { get; init; } = default!;

		public decimal Fee { get; init; }

		public decimal FeeRate { get; init; }

		public decimal OrderAmount { get; init; }

		public string OrderNo { get; init; } = default!;

		public string Status { get; init; } = default!;

		public string ToAddress { get; init; } = default!;

		public string TranscationHash { get; init; } = default!;

		[JsonPropertyName("txnSign")]
		public string TranscationSigningCode { get; init; } = default!;
	}
}
