using System;

namespace Tgs.Payment.LeePay.Services
{
	public record SaveDepositReturnCommand(Guid FormId, LeePayDepositReturn ReturnData);
}
