using System;

namespace Tgs.Payment.LeePay
{
	public record WithdrawFormInfo(
		string PlayerCode,
		Guid BrandId,
		Guid? BrandPaymentId,
		decimal Amount,
		int Status,
		Guid BankInfoId,
		string? BankInfo,
		LeePayWithdrawApplyInfo? ApplyInfo,
		string? PaymentReturn = null);
}
