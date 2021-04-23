#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。

namespace Tgs.Payment.LeePay
{
	internal record LeePayResponse<TData>
	{
		public string Code { get; init; }

		public string Message { get; init; }

		public TData? Data { get; init; }
	}
}

#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
