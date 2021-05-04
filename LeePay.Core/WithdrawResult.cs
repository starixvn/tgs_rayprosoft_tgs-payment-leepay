#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。

namespace Tgs.Payment.LeePay
{
	internal class WithdrawResult
	{
		public string Channel { get; set; }

		public string ChannelCurrency { get; set; }

		public decimal ChannelRate { get; set; }

		public string CustomOrderNo { get; set; }

		public string OrderNo { get; set; }

		public decimal UsdtAmount { get; set; }
	}
}

#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
