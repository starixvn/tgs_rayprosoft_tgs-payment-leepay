using System;

namespace Tgs.Payment.LeePay
{
	public record LeePayWithdrawResponse(
		string Code,
		string? Message,
		string OrderNo,
		Guid CustomOrderNo,
		decimal UsdtAmount,
		string ChannelCurrency,
		decimal ChannelRate,
		string? Channel);
}
