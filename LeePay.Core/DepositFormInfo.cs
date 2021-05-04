using System;

namespace Tgs.Payment.LeePay
{
	public record DepositFormInfo(
		string PlayerCode,
		Guid BrandId,
		Guid BrandPaymentId,
		decimal Amount,
		LeePayDepositApplyInfo? ApplyInfo,
		string? PaymentReturn = null);
}
