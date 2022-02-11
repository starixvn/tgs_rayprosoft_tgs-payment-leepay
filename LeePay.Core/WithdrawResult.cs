using System;

namespace Tgs.Payment.LeePay;

internal class WithdrawResult
{
	public string Channel { get; set; } = default!;

	public string ChannelCurrency { get; set; } = default!;

	public decimal ChannelRate { get; set; }

	public string CustomOrderNo { get; set; } = default!;

	public Guid OrderId { get; set; }

	public string OrderNo { get; set; } = default!;

	public decimal UsdtAmount { get; set; }
}
