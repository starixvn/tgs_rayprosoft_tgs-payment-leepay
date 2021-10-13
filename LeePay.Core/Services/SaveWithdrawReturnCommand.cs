using System;
using Tgs.Payment.Clients;

namespace Tgs.Payment.LeePay.Services
{
	public record SaveWithdrawReturnCommand(
		Guid FormId,
		PaymentFlowState State,
		LeePayWithdrawReturn ReturnData,
		decimal? ActualAmount);
}
