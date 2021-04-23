namespace Tgs.Payment.LeePay
{
	public record LeePayApplyInfo(
		LeePayDepositSettings Settings,
		LeePayDepositResponse Response);
}
