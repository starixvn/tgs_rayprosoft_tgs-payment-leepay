namespace Tgs.Payment.LeePay;

public record LeePayDepositApplyInfo(
	LeePayDepositSettings Settings,
	LeePayDepositResponse Response);
