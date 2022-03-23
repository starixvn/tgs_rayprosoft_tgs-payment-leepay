using System;

namespace Tgs.Payment.LeePay;

public class DepositOrderDetailResult
{
	public string Channel { get; init; } = default!;

	public decimal ChannelAmount { get; init; }

	public string ChannelCurrency { get; init; } = default!;

	public decimal ChannelCurrencyRate { get; init; }

	public DateTimeOffset CreatedAt { get; init; }

	public string CustomOrderNo { get; init; } = default!;

	public decimal Fee { get; init; }

	public decimal FeeRate { get; init; }

	public decimal OrderAmount { get; init; }

	public string OrderNo { get; init; } = default!;

	public string Status { get; init; } = default!;

	public string ToAddress { get; init; } = default!;

	public string TransactionHash { get; init; } = default!;

	public string Uid { get; init; } = default!;
}
