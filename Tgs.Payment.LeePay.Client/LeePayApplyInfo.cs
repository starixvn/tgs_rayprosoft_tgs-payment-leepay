using System;

namespace Tgs.Payment.LeePay
{
	public record LeePayApplyInfo(
		string SystemId,
		string SystemKey,
		Guid CustomOrderNo,
		string ChannelCurrency,
		string ReturnUrl,
		string? Channel);
}
