using System;

namespace Tgs.Payment.LeePay
{
	public record LeePayDepositReturn(
		string CustomOrderNo,
		string? NotifyCustomMessage,
		string OrderNo,
		string Uid,
		string Status,
		decimal OrderAmount,
		decimal FeeRate,
		decimal Fee,
		string ToAddress,
		string TransactionHash,
		DateTimeOffset CreatedAt,
		string Channel,
		string ActualAmount,
		decimal ChannelAmount,
		string ChannelCurrency,
		decimal ChannelCurrencyRate);
}
