#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。

using System.Text.Json.Serialization;

namespace Tgs.Payment.LeePay
{
	internal class DepositOrder
	{
		public decimal Amount { get; set; }

		public string? Channel { get; set; }

		public string ChannelCurrency { get; set; }

		[JsonPropertyName("customMsg")]
		public string? CustomMessage { get; set; }

		public string CustomOrderNo { get; set; }

		public int? DepositLimitTime { get; set; }

		public string? NotifyCustomMessage { get; set; }

		public string? NotifyUrl { get; set; }

		public string? ReturnUrl { get; set; }

		[JsonPropertyName("txnSign")]
		public string? TranscationSigningCode { get; set; }
	}
}

#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
