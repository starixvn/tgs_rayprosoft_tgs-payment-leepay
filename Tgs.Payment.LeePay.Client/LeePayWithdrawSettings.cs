using System;

namespace Tgs.Payment.LeePay
{
	public record LeePayWithdrawSettings(
		string SystemId,
		string SystemKey,
		Guid CustomOrderNo,
		string DestinationAddress,
		string ChannelCurrency,
		string? Channel);
}
