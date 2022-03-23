using System;
using Tgs.Payment.Clients;

namespace Tgs.Payment.LeePay;

public record WithdrawFormInfo(
	Guid Id,
	string? UserId,
	UserType UserType,
	Guid BrandId,
	Guid? BrandPaymentId,
	decimal Amount,
	PaymentFlowState Status,
	Guid BankInfoId,
	string? BankInfo,
	LeePayWithdrawApplyInfo? ApplyInfo,
	string? PaymentReturn = null);
