namespace Tgs.Payment.LeePay
{
	public record LeePayQueryExchangeRateSettings(
		string SystemId,
		string SystemKey,
		string From,
		string To);
}