using System;

namespace Tgs.Payment.LeePay
{
	public record LeePayWithdrawReturn(
		string Channel,
		DateTimeOffset CreatedAt,
		string CustomOrderNo,
		decimal Fee,
		decimal FeeRate,
		string? NotifyCustomMessage,
		decimal OrderAmount,
		string OrderNo,
		string Status,
		string ToAddress,
		string TransactionHash);
}
