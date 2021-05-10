using System;
using System.Text.Json.Serialization;

namespace Tgs.Payment.LeePay.ViewModels
{
	public record DepositResponseViewModel
	{
		public string CustomOrderNo { get; init; } = default!;

		[JsonPropertyName("notifyCustomMsg")]
		public string? NotifyCustomMessage { get; init; }

		public string OrderNo { get; init; } = default!;

		public string Uid { get; init; } = default!;

		public string Status { get; init; } = default!;

		public decimal OrderAmount { get; init; }

		public decimal FeeRate { get; init; }

		public decimal Fee { get; init; }

		public string ToAddress { get; init; } = default!;

		public string TransactionHash { get; init; } = default!;

		public DateTimeOffset CreatedAt { get; init; } = default!;

		public string Channel { get; init; } = default!;

		public string ActualAmount { get; init; } = default!;

		[JsonPropertyName("txnSign")]
		public string TranscationSigningCode { get; init; } = default!;
	}
}
