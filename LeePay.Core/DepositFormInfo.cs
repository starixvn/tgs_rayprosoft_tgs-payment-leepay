using System;

namespace Tgs.Payment.LeePay
{
	public record DepositFormInfo(
		string PlayerCode,
		Guid BrandId,
		Guid BrandPaymentId,
		decimal Amount,
		LeePayApplyInfo? ApplyInfo,
		string? PaymentReturn = null);
}
