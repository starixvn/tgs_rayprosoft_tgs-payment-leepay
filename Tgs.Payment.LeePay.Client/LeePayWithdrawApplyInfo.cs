namespace Tgs.Payment.LeePay
{
	public record LeePayWithdrawApplyInfo(
		LeePayWithdrawSettings Settings,
		LeePayWithdrawResponse Response);
}
