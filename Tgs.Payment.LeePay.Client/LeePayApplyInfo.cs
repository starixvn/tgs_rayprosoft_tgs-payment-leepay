#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。

using System;

namespace Tgs.Payment.LeePay.Client
{
	public class LeePayApplyInfo
	{
		public string SystemId { get; set; }

		public string SystemKey { get; set; }

		public Guid CustomOrderNo { get; set; }

		public string ChannelCurrency { get; set; }

		public string ReturnUrl { get; set; }

		public string? Channel { get; set; }
	}
}

#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
