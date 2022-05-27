namespace Tgs.Payment.LeePay.Services;

public record SaveWithdrawReturnCommand(
	Guid FormId,
	LeePayWithdrawReturn ReturnData,
	decimal? ActualAmount);
