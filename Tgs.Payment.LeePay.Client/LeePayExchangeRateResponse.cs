namespace Tgs.Payment.LeePay
{
	public record LeePayExchangeRateResponse(
		string Code,
		string? Message = null,
		decimal? BuyPrice = null,
		decimal? SellPrice = null,
		string? BizTime = null);
}