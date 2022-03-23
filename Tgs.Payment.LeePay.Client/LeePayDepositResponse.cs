using System;

namespace Tgs.Payment.LeePay;

public record LeePayDepositResponse(
	string Code,
	string? Message = null,
	string? OrderId = null,
	string? OrderNo = null,
	Guid? CustomOrderNo = null,
	string? WalletAddress = null,
	string? PaymentUrl = null,
	decimal? UsdtAmount = null,
	string? ChannelCurrency = null,
	decimal? ChannelRate = null,
	string? Channel = null,
	string? ResponseContent = null);
