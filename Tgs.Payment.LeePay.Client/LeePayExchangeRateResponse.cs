namespace Tgs.Payment.LeePay
{
	public record LeePayExchangeRateResponse(
		string Code,
		decimal? BuyPrice = null,
		decimal? SellPrice = null,
		string? BizTime = null,
		string? ClientCode = null,
		string? Message = null,
		string? TraceId = null,
		string? DisplayMessage = null,
		string? Timestamp = null,
		string? Path = null);
}