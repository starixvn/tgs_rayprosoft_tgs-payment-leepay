namespace Tgs.Payment.LeePay
{
	public record LeePayExchangeRateSettings(
		string SystemId,
		string SystemKey,
		string From,
		string To);
}