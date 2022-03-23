using System;

namespace Tgs.Payment.LeePay;

public record LeePayDepositSettings(
	string SystemId,
	string SystemKey,
	Guid CustomOrderNo,
	string ChannelCurrency,
	string ReturnUrl,
	string? Channel);
