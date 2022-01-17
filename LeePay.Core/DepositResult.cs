namespace Tgs.Payment.LeePay;

internal record DepositResult
{
	public string OrderId { get; init; } = default!;

	public string OrderNo { get; init; } = default!;

	public string CustomOrderNo { get; init; } = default!;

	public string WalletAddress { get; init; } = default!;

	public string PaymentUrl { get; init; } = default!;

	public decimal UsdtAmount { get; init; }

	public string ChannelCurrency { get; init; } = default!;

	public decimal ChannelRate { get; init; }

	public string Channel { get; init; } = default!;
}
