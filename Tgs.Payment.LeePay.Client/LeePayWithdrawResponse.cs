using System;

namespace Tgs.Payment.LeePay;

public record LeePayWithdrawResponse(
	string Code,
	string? Message = null,
	string? OrderNo = null,
	Guid? CustomOrderNo = null,
	decimal? UsdtAmount = null,
	string? ChannelCurrency = null,
	decimal? ChannelRate = null,
	string? Channel = null,
	string? ResponseContent = null);
