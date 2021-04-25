#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。

using System;
using System.Text.Json.Serialization;

namespace Tgs.Payment.LeePay.ViewModels
{
	public record DepositReponseViewModel
	{
		public string CustomOrderNo { get; init; }

		[JsonPropertyName("notifyCustomMsg")]
		public string NotifyCustomMessage { get; init; }

		public string OrderNo { get; init; }

		public string Uid { get; init; }

		public string Status { get; init; }

		public decimal OrderAmount { get; init; }

		public decimal FeeRate { get; init; }

		public decimal Fee { get; init; }

		public string ToAddress { get; init; }

		public string TransactionHash { get; init; }

		public DateTimeOffset CreatedAt { get; init; }

		public string Channel { get; init; }

		public string ActualAmount { get; init; }

		[JsonPropertyName("txnSign")]
		public string TranscationSigningCode { get; init; }
	}
}

#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
