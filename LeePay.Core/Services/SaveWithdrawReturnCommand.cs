using System;

namespace Tgs.Payment.LeePay.Services
{
	public record SaveWithdrawReturnCommand(Guid FormId, LeePayWithdrawReturn ReturnData);
}
