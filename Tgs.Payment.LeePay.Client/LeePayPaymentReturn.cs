using System;
using Tgs.Payment.LeePay.Client;

namespace Tgs.Payment.LeePay
{
	public record LeePayPaymentReturn(
		string Code,
		string? Message,
		string OrderId,
		string OrderNo,
		Guid CustomOrderNo,
		string WalletAddress,
		string PaymentUrl,
		decimal UsdtAmount,
		string ChannelCurrency,
		decimal ChannelRate,
		string? Channel);
}
