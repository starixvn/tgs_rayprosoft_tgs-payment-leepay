using System;
using Tgs.Payment.Clients;

namespace Tgs.Payment.LeePay
{
	public record DepositFormInfo(
		Guid Id,
		string PlayerCode,
		Guid BrandId,
		Guid BrandPaymentId,
		decimal Amount,
		PaymentFlowState Status,
		LeePayDepositApplyInfo? ApplyInfo,
		string? PaymentReturn = null);
}
