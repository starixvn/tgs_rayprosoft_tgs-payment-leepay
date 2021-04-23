#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。

using System;

namespace Tgs.Payment.LeePay.Client
{
	public record LeePayPaymentReturn
	{
		public string Code { get; init; }

		public string? Message { get; init; }

		public string OrderId { get; init; }

		public string OrderNo { get; init; }

		public Guid CustomOrderNo { get; init; }

		public string WalletAddress { get; init; }

		public string PaymentUrl { get; init; }

		public decimal UsdtAmount { get; init; }

		public string ChannelCurrency { get; init; }

		public decimal ChannelRate { get; init; }

		public string? Channel { get; init; }
	}
}

#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
