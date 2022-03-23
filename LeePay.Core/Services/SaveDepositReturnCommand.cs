using System;
using Tgs.Payment.Clients;

namespace Tgs.Payment.LeePay.Services;

public record SaveDepositReturnCommand(
	Guid FormId,
	PaymentFlowState State,
	LeePayDepositReturn ReturnData);
