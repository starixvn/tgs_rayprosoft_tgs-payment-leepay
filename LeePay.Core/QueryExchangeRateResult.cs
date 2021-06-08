namespace Tgs.Payment.LeePay
{
	internal class QueryExchangeRateResult
	{
		/// <summary>
		/// 交易所收購價格，建議用來充值使用
		/// </summary>
		public decimal BuyPrice { get; set; } = default!;

		/// <summary>
		/// 交易所賣出價格，建議用來出款使用
		/// </summary>
		public decimal SellPrice { get; set; } = default!;

		/// <summary>
		/// 價格更新日期 ex:2021-06-04T09:22:00.000Z
		/// </summary>
		public string BizTime { get; set; } = default!;
	}
}