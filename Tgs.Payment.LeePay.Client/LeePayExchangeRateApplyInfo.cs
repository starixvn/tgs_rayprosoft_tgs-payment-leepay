namespace Tgs.Payment.LeePay
{
	public record LeePayExchangeRateApplyInfo(
		LeePayExchangeRateSettings Settings,
		LeePayExchangeRateResponse Response);
}